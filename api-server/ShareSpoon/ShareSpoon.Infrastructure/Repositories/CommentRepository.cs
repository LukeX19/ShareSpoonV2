using Microsoft.EntityFrameworkCore;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Interactions;
using ShareSpoon.Domain.Models.Recipes;
using ShareSpoon.Infrastructure.Exceptions;
using ShareSpoon.Infrastructure.Repositories.BasicRepositories;

namespace ShareSpoon.Infrastructure.Repositories
{
    public class CommentRepository : BaseRepositoryEF<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Comment> GetCommentById(long id, CancellationToken ct = default)
        {
            return await _context.Comments.AsSplitQuery()
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id, ct)
                ?? throw new EntityNotFoundException(nameof(Comment), id);
        }

        public async Task<PagedResponseDto<Comment>> GetCommentsByRecipeId(long recipeId, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe == null)
            {
                throw new EntityNotFoundException(nameof(Recipe), recipeId);
            }

            var comments = await _context.Comments.AsSplitQuery()
                .Include(c => c.User)
                .Where(c => c.RecipeId == recipeId)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            var count = await _context.Comments.Where(c => c.RecipeId == recipeId).CountAsync(ct);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PagedResponseDto<Comment>(comments, pageIndex, totalPages);
        }
    }
}
