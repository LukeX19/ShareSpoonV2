namespace ShareSpoon.App.Exceptions
{
    public class EmptyIngredientsListException : Exception
    {
        private const string MessageTemplate = "No valid ingredients provided. A recipe must include at least one valid ingredient.";

        public EmptyIngredientsListException()
            : base(string.Format(MessageTemplate)) { }

        public EmptyIngredientsListException(Exception innerException)
            : base(string.Format(MessageTemplate), innerException) { }
    }
}
