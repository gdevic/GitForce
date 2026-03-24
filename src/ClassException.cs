using System;

namespace GitForce
{
    public class ClassException : Exception
    {
        private readonly string msg;

        public override string Message
        {
            get { return msg; }
        }

        public ClassException(string message)
        {
            msg = message;
        }
    }
}
