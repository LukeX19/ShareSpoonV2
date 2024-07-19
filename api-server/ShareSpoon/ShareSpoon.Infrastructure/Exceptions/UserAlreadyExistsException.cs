namespace ShareSpoon.Infrastructure.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        private const string MessageTemplate = "A user account is already associated with email {0}.";

        public UserAlreadyExistsException()
            : base() { }

        public UserAlreadyExistsException(string email)
            : base(string.Format(MessageTemplate, email)) { }

        public UserAlreadyExistsException(string email, Exception innerException)
            : base(string.Format(MessageTemplate, email), innerException) { }
    }
}
