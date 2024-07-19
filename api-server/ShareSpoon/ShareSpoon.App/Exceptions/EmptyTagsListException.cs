namespace ShareSpoon.App.Exceptions
{
    public class EmptyTagsListException : Exception
    {
        private const string MessageTemplate = "No valid tags provided. A recipe must include at least one valid tag.";

        public EmptyTagsListException()
            : base(string.Format(MessageTemplate)) { }

        public EmptyTagsListException(Exception innerException)
            : base(string.Format(MessageTemplate), innerException) { }
    }
}
