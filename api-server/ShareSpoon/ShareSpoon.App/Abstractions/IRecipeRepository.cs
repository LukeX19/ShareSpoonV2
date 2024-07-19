using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.App.Abstractions
{
    public interface IRecipeRepository : IBaseRepository<Recipe>
    {
        Task<Recipe> GetRecipeById(long recipeId, CancellationToken ct = default);
        Task<RecipeWithInteractionsResponseDto> GetRecipeWithInteractionsById(string userId, long recipeId, CancellationToken ct = default);
        Task<PagedResponseDto<RecipeWithInteractionsResponseDto>> GetAllRecipes(string userId, int pageIndex, int pageSize, CancellationToken ct = default);
        Task<PagedResponseDto<RecipeWithInteractionsResponseDto>> GetRecipesByUserId(string userId, int pageIndex, int pageSize, CancellationToken ct = default);
        Task<PagedResponseDto<RecipeWithInteractionsResponseDto>> GetLikedRecipesByUserId(string userId, int pageIndex, int pageSize, CancellationToken ct = default);
        Task<CustomPagedResponseDto<RecipeWithInteractionsResponseDto>> SearchRecipes(string? input, bool? promotedUsers,
            List<DifficultyLevel>? difficulties, List<long>? tagIds, string userId, int pageIndex, int pageSize, CancellationToken ct = default);
    }
}
