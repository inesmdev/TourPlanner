using System;

namespace TourPlanner.Exceptions
{
    public class CouldNotCreateTourException : Exception
    {
        public CouldNotCreateTourException()
        {
        }

        public CouldNotCreateTourException(string message)
            : base(message)
        {
        }

        public CouldNotCreateTourException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
