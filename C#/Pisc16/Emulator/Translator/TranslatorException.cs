using System;

namespace Pisc16
{
    public class TranslatorException : Exception
    {
        public TranslatorException(string command, int line, string message)
        {
            Message = command + ": " + message;
        }

        public new string Message
        {
            get;
            private set;
        }
    }
}
