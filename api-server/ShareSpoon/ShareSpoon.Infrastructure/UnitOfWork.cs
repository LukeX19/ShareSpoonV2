using ShareSpoon.App.Abstractions;

namespace ShareSpoon.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository UserRepository { get; private set; }
        public IIngredientRepository IngredientRepository { get; private set; }
        public ITagRepository TagRepository { get; private set; }
        public IRecipeRepository RecipeRepository { get; private set; }
        public ILikeRepository LikeRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }

        public UnitOfWork(AppDbContext context, IUserRepository userRepository,
            IIngredientRepository ingredientRepository, ITagRepository tagRepository, IRecipeRepository recipeRepository,
            ILikeRepository likeRepository, ICommentRepository commentRepository)
        {
            _context = context;
            UserRepository = userRepository;
            IngredientRepository = ingredientRepository;
            TagRepository = tagRepository;
            RecipeRepository = recipeRepository;
            LikeRepository = likeRepository;
            CommentRepository = commentRepository;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
