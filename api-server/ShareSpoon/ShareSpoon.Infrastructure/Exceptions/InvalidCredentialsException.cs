namespace ShareSpoon.Infrastructure.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        private const string MessageTemplate = "The provided credentials are invalid.";

        public InvalidCredentialsException()
            : base(string.Format(MessageTemplate)) { }

        public InvalidCredentialsException(Exception innerException)
            : base(string.Format(MessageTemplate), innerException) { }
    }
}
