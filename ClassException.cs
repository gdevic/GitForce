using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitForce
{
    public class ClassException : Exception
    {
        private readonly string _msg;

        public override string Message
        {
            get { return _msg; }
        }

        public ClassException(string message)
        {
            _msg = message;
        }
    }
}
