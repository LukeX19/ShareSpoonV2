using ShareSpoon.Domain.Models.Interactions;

namespace ShareSpoon.App.Abstractions
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
        Task<long> GetLikesCounterByRecipeId(long id, CancellationToken ct = default);
        Task DeleteLike(string userId, long recipeId, CancellationToken ct = default);
    }
}
