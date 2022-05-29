using System;

namespace TourPlanner.Exceptions
{
    public class CouldNotParseException : Exception
    {
        public CouldNotParseException()
        {
        }

        public CouldNotParseException(string message)
            : base(message)
        {
        }

        public CouldNotParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
