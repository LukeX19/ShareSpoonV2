namespace ShareSpoon.Infrastructure.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string MessageTemplate = "The {0} with id {1} was not found.";

        public EntityNotFoundException()
            : base() { }

        public EntityNotFoundException(string entityType, long entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public EntityNotFoundException(string entityType, long entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }

        public EntityNotFoundException(string entityType, string entityId)
            : base(string.Format(MessageTemplate, entityType, entityId)) { }

        public EntityNotFoundException(string entityType, string entityId, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityId), innerException) { }
    }
}
