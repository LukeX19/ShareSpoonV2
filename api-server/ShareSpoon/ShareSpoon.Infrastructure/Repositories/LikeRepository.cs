using Microsoft.EntityFrameworkCore;
using ShareSpoon.App.Abstractions;
using ShareSpoon.Domain.Models.Interactions;
using ShareSpoon.Domain.Models.Recipes;
using ShareSpoon.Infrastructure.Exceptions;
using ShareSpoon.Infrastructure.Repositories.BasicRepositories;

namespace ShareSpoon.Infrastructure.Repositories
{
    public class LikeRepository : BaseRepositoryEF<Like>, ILikeRepository
    {
        public LikeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<long> GetLikesCounterByRecipeId(long recipeId, CancellationToken ct = default)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe == null)
            {
                throw new EntityNotFoundException(nameof(Recipe), recipeId);
            }

             return await _context.Likes.CountAsync(l => l.RecipeId == recipeId, ct);
        }

        public async Task DeleteLike(string userId, long recipeId, CancellationToken ct = default)
        {
            var likeToDelete = _context.Likes.FirstOrDefault(l => l.UserId == userId && l.RecipeId == recipeId);
            if (likeToDelete == null)
            {
                throw new LikeNotFoundException(userId, recipeId);
            }
            _context.Likes.Remove(likeToDelete);
            await _context.SaveChangesAsync(ct);
        }
    }
}
