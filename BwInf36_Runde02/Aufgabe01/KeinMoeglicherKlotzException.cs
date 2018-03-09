using System;

namespace Aufgabe01
{
    public class KeinMoeglicherKlotzException : Exception
    {
        public KeinMoeglicherKlotzException()
        {
        }

        public KeinMoeglicherKlotzException(string message)
            : base(message)
        {
        }

        public KeinMoeglicherKlotzException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}