using System;
using System.Threading;
using System.Windows.Forms;

namespace GitForce
{
    /// <summary>
    /// Last-resort reporting for exceptions that no other code handled.
    ///
    /// Without this, a fault escaping a WinForms event handler shows the bare .NET crash
    /// dialog, and a fault on a background thread kills the process with nothing written down
    /// at all. Install() hooks both cases and reports them the same way: a short dialog naming
    /// the error and the first line of GitForce code on the stack, with the complete exception
    /// sent to the log window and to the log file when the app was started with '--log'.
    /// </summary>
    static class ClassFault
    {
        /// <summary>
        /// Guards against a fault raised by the fault reporter itself, which would otherwise
        /// recurse until the stack runs out.
        /// </summary>
        private static bool inFaultReport;

        /// <summary>
        /// Subscribes the global handlers. Call this before any other startup work so that
        /// faults raised during initialization are covered as well.
        ///
        /// Application.SetUnhandledExceptionMode() is deliberately not called: the default
        /// routing already reaches these handlers, while forcing CatchException would also
        /// swallow exceptions that should break into the debugger during development.
        /// </summary>
        public static void Install()
        {
            Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        /// <summary>
        /// Handles an exception that escaped a WinForms event handler. WinForms can normally
        /// carry on afterwards, so we report the fault and let the application keep running.
        /// </summary>
        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Report(e.Exception, false);
        }

        /// <summary>
        /// Handles an exception on a thread that has no handler of its own. Unlike the WinForms
        /// case these are normally fatal and the process is already on its way down.
        /// </summary>
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Report(e.ExceptionObject as Exception, e.IsTerminating);
        }

        /// <summary>
        /// Reports a fault that nothing else handled. The dialog is deliberately short: what
        /// went wrong, and the first line of GitForce code on the stack, which is where to start
        /// looking. The complete exception, with its stack trace and any inner exceptions, goes
        /// to the log window and to the log file.
        /// </summary>
        private static void Report(Exception ex, bool isFatal)
        {
            if (inFaultReport)          // A fault while reporting a fault: give up quietly
                return;
            inFaultReport = true;
            try
            {
                string summary = Describe(ex);

                // Record it before showing the dialog, so it survives even if the dialog fails.
                // The summary goes to the status pane, which forwards it to the log on its own,
                // and the full trace goes to the log alone: sending both to both would print
                // the same text twice in the log window.
                try
                {
                    App.PrintStatusMessage(summary, MessageType.Error);
                    if (ex != null)
                        App.PrintLogMessage(ex.ToString(), MessageType.Error);
                }
                catch (Exception) { }

                MessageBox.Show(summary + Environment.NewLine + Environment.NewLine +
                    (isFatal ? "GitForce has to close." : "GitForce will try to keep running.") +
                    " Full details are in the log window (View > Log Window).",
                    "GitForce error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                inFaultReport = false;
            }
        }

        /// <summary>
        /// Builds the short description shown in the fault dialog: the underlying error, plus
        /// the first stack frame belonging to GitForce. That frame names the source file and the
        /// line number whenever the matching .pdb sits next to the executable.
        /// </summary>
        private static string Describe(Exception ex)
        {
            if (ex == null)
                return "Unknown error: the fault carried no exception details.";

            // Report the innermost exception: wrappers such as TargetInvocationException
            // describe the plumbing rather than the actual cause
            Exception cause = ex.GetBaseException();
            string summary = cause.GetType().Name + ": " + cause.Message;

            string stack = cause.StackTrace ?? string.Empty;
            foreach (string frame in stack.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (frame.Contains("GitForce."))
                    return summary + Environment.NewLine + ShortenFrame(frame.Trim());
            }
            return summary;
        }

        /// <summary>
        /// Reduces a stack frame to the part worth reading: the method, and the source file and
        /// line if the frame carries them. Everything else is dropped, because the full path
        /// stored in the symbols belongs to the machine that built the release.
        ///
        /// The two runtimes format frames differently, so both are handled:
        ///   .NET  "at GitForce.X.Y(String s) in C:\src\File.cs:line 506"
        ///   Mono  "at GitForce.X.Y (System.String s) [0x00024] in /src/File.cs:506"
        /// and Mono with no symbols loaded substitutes the assembly MVID for the path:
        ///   Mono  "at GitForce.X.Y (System.String s) [0x00024] in &lt;72a0cb...&gt;:0"
        /// </summary>
        private static string ShortenFrame(string frame)
        {
            // Mono inserts an IL offset such as "[0x00024]", which says nothing to a reader
            // who does not have a disassembler in front of them
            int il = frame.IndexOf(" [0x", StringComparison.Ordinal);
            if (il > 0)
            {
                int close = frame.IndexOf(']', il);
                if (close > il)
                    frame = frame.Remove(il, close - il + 1);
            }

            int at = frame.IndexOf(" in ", StringComparison.Ordinal);
            if (at < 0)
                return frame;                   // No source information in this frame

            string method = frame.Substring(0, at);
            string source = frame.Substring(at + 4);

            // With no symbols loaded Mono puts the assembly MVID here, as in "<72a0cb...>:0".
            // That names nothing anyone can open, so drop the clause rather than print it.
            if (source.StartsWith("<", StringComparison.Ordinal))
                return method;

            // What remains is "<path>:line <n>" on .NET or "<path>:<n>" on Mono. Only treat the
            // tail as a line number if it really is one, so a path that has no line number
            // appended cannot be split at the drive letter's colon.
            int colon = source.LastIndexOf(':');
            if (colon > 0)
            {
                int number;
                string tail = source.Substring(colon + 1).Replace("line ", "").Trim();
                if (int.TryParse(tail, out number))
                    return method + " in " + FileNameOnly(source.Substring(0, colon)) + ":" + number;
            }
            return method + " in " + FileNameOnly(source);
        }

        /// <summary>
        /// Returns the file name portion of a path. Path.GetFileName is not used here because
        /// symbols built on Windows carry backslash-separated paths that would still be read on
        /// Mono, where the backslash is not a directory separator.
        /// </summary>
        private static string FileNameOnly(string path)
        {
            int cut = path.LastIndexOfAny(new[] { '/', '\\' });
            return cut < 0 ? path : path.Substring(cut + 1);
        }
    }
}
