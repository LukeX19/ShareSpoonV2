using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Interactions;

namespace ShareSpoon.App.Abstractions
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<Comment> GetCommentById(long id, CancellationToken ct = default);
        Task<PagedResponseDto<Comment>> GetCommentsByRecipeId(long recipeId, int pageIndex, int pageSize, CancellationToken ct = default);
    }
}
