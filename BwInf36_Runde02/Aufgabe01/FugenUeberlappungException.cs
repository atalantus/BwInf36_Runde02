using System;

namespace Aufgabe01
{
    public class FugenUeberlappungException : Exception
    {
        public FugenUeberlappungException()
        {
        }

        public FugenUeberlappungException(string message)
            : base(message)
        {
        }

        public FugenUeberlappungException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}