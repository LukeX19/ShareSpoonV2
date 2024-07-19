namespace ShareSpoon.Infrastructure.Exceptions
{
    public class LikeNotFoundException : Exception
    {
        private const string MessageTemplate = "The Like for User id {0} on Recipe id {1} was not found.";

        public LikeNotFoundException()
            : base() { }

        public LikeNotFoundException(string userId, long recipeId)
            : base(string.Format(MessageTemplate, userId, recipeId)) { }

        public LikeNotFoundException(string userId, long recipeId, Exception innerException)
            : base(string.Format(MessageTemplate, userId, recipeId), innerException) { }
    }
}
