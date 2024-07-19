namespace ShareSpoon.App.Abstractions
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IIngredientRepository IngredientRepository { get; }
        public ITagRepository TagRepository { get; }
        public IRecipeRepository RecipeRepository { get; }
        public ILikeRepository LikeRepository { get; }
        public ICommentRepository CommentRepository { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task SaveChangesAsync();
    }
}
