using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GitForce
{
    public partial class FormDownload : Form
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly WebClient client = new WebClient();
        private readonly AutoResetEvent inProgress = new AutoResetEvent(false);
        private bool isCompleted;
        private readonly string webPage;
        private readonly string regEx;
        private readonly bool mergeWebPath;

        /// <summary>
        /// Local path and name of the file that was downloaded
        /// </summary>
        public string TargetFile { get; private set; }

        public FormDownload(string title, string webPageArg, string regExArg, bool mergeWebPathArg)
        {
            InitializeComponent();
            Text = title;
            webPage = webPageArg;
            regEx = regExArg;
            mergeWebPath = mergeWebPathArg;
            Work();
        }

        /// <summary>
        /// Initiate download of a file through a separate thread
        /// </summary>
        private void Work()
        {
            labelInfo.Text = "Searching for the latest installer...";

            worker.DoWork += delegate
            {
                string webName;
                if(GetTargetFile(out webName))
                {
                    UIThread(() => labelInfo.Text = "Downloading file " + webName);
                    UIThread(() => btCancel.Enabled = true);

                    // Download the remote file into local temp directory and keep the file name
                    TargetFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(webName));

                    string response = Download(TargetFile, webName);
                    if (response == String.Empty)
                    {
                        UIThread(() => labelInfo.Text = "SUCCESS");
                    }
                    else
                    {
                        UIThread(() => labelInfo.Text = response);                        
                    }
                }
                else
                {
                    UIThread(() => labelInfo.Text = webName);
                }
            };
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// This is a final function that gets invoked on a job completion
        /// </summary>
        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Remove temporary file if we did not complete the download
            if(!isCompleted)
            {
                File.Delete(TargetFile);
                DialogResult = DialogResult.Cancel;
            }
            else
                DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// A helper function that lets us update controls in the GUI thread
        /// </summary>
        private void UIThread(Action code)
        {
            if (InvokeRequired)
                BeginInvoke(code);
            else
                code.Invoke();
        }

        /// <summary>
        /// Returns the web path of the target file to download.
        /// Returns String.Empty if the file could not be itentified.
        /// </summary>
        private bool GetTargetFile(out string targetFile)
        {
            try
            {
                WebRequest request = WebRequest.Create(webPage);
                request.Timeout = 4000;
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                StringBuilder file = new StringBuilder();
                file.Append(reader.ReadToEnd());

                // Find the first executable
                Regex r = new Regex(regEx, RegexOptions.Compiled);
                if (r.IsMatch(file.ToString()))
                {
                    targetFile = r.Match(file.ToString()).Result("${file}");

                    // Merge the root of the target web page with the name of the file to download
                    if (mergeWebPath)
                        targetFile = webPage + targetFile;

                    return true;
                }
                targetFile = "Could not find a target file matching a pattern!";
            }
            catch (Exception ex)
            {
                targetFile = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// Download a file from the web path
        /// </summary>
        private string Download(string target, string webPath)
        {
            try
            {
                UriBuilder url = new UriBuilder(webPath);

                client.Proxy = null;
                client.DownloadProgressChanged += ClientDownloadProgressChanged;
                client.DownloadFileCompleted += ClientDownloadFileCompleted;
                client.DownloadFileAsync(url.Uri, target);

                inProgress.WaitOne();        // Wait until download completes or user cancels
                if (isCompleted==false)
                    return String.Empty;
                return "Download has been cancelled.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Callback by the web client class when the download finish (completed or cancelled)
        /// </summary>
        void ClientDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Thread.Sleep(1500);     // A short pause for a better visual flow

            inProgress.Set();
            isCompleted = !e.Cancelled;
        }

        /// <summary>
        /// Callback by the web client when a new piece of file has been incrementally downloaded
        /// </summary>
        void ClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string progress = String.Format("Downloaded {0} of {1} Kb", e.BytesReceived / 1024, e.TotalBytesToReceive / 1024);
            UIThread(() => labelProgress.Text = progress);
            UIThread(() => progressBar.Maximum = (int) e.TotalBytesToReceive);
            UIThread(() => progressBar.Value = (int) e.BytesReceived);
        }

        /// <summary>
        /// User clicked on the Cancel button. Abort the download.
        /// </summary>
        private void BtCancelClick(object sender, EventArgs e)
        {
            client.CancelAsync();
        }
    }
}
