using ShareSpoon.Domain.Models.Ingredients;

namespace ShareSpoon.App.Abstractions
{
    public interface IIngredientRepository : IBaseRepository<Ingredient>
    {
        Task<Ingredient> CreateIngredient(Ingredient newIngredient, CancellationToken ct = default);
        Task<ICollection<Ingredient>> GetIngredientsByIds(ICollection<long> ids, CancellationToken ct = default);
        Task<ICollection<Ingredient>> SearchIngredientsByName(string name, CancellationToken ct=default);
    }
}
