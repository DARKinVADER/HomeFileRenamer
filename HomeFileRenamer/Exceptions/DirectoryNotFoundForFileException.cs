using System;

namespace HomeFileRenamer.Exceptions
{
    public class DirectoryNotFoundForFileException : Exception
    {
        public DirectoryNotFoundForFileException(string message) : base(message)
        {
        }

        public DirectoryNotFoundForFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
