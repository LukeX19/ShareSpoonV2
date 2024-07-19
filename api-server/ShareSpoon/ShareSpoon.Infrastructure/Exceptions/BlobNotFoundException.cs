namespace ShareSpoon.Infrastructure.Exceptions
{
    public class BlobNotFoundException : Exception
    {
        private const string MessageTemplate = "The Blob {0} was not found.";

        public BlobNotFoundException()
            : base() { }

        public BlobNotFoundException(string blobName)
            : base(string.Format(MessageTemplate, blobName)) { }

        public BlobNotFoundException(string blobName, Exception innerException)
            : base(string.Format(MessageTemplate, blobName), innerException) { }
    }
}
