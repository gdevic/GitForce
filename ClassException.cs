using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace git4win
{
    public class ClassException : Exception
    {
        public string Msg;

        public ClassException(string message)
        {
            Msg = message;
        }
    }
}
