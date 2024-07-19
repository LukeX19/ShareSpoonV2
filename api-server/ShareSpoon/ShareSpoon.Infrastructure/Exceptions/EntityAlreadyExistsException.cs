namespace ShareSpoon.Infrastructure.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        private const string MessageTemplate = "The {0} with id {1} already exists.";
        private const string MessageTemplate2 = "One {0} with similar name already exists.";

        public EntityAlreadyExistsException()
            : base() { }

        public EntityAlreadyExistsException(string entityType, long entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public EntityAlreadyExistsException(string entityType, long entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }

        public EntityAlreadyExistsException(string entityType)
            : base(string.Format(MessageTemplate2, entityType)) { }

        public EntityAlreadyExistsException(string entityType, Exception innerException)
            : base(string.Format(MessageTemplate2, entityType), innerException) { }
    }
}
