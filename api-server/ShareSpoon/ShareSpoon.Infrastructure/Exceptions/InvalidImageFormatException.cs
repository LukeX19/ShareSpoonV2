namespace ShareSpoon.Infrastructure.Exceptions
{
    public class InvalidImageFormatException : Exception
    {
        private const string MessageTemplate = "The provided file extension is not supported. Please use .png, .jpg or .jpeg files instead.";

        public InvalidImageFormatException()
            : base(string.Format(MessageTemplate)) { }

        public InvalidImageFormatException(Exception innerException)
            : base(string.Format(MessageTemplate), innerException) { }
    }
}
