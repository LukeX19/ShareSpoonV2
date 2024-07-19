using Microsoft.EntityFrameworkCore;
using ShareSpoon.App.Abstractions;
using ShareSpoon.Domain.Models.Ingredients;
using ShareSpoon.Infrastructure.Exceptions;
using ShareSpoon.Infrastructure.Repositories.BasicRepositories;

namespace ShareSpoon.Infrastructure.Repositories
{
    public class IngredientRepository : BaseRepositoryEF<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Ingredient> CreateIngredient(Ingredient newIngredient, CancellationToken ct = default)
        {
            var exists = await _context.Ingredients
                .AnyAsync(i => i.Name.ToLower() == newIngredient.Name.ToLower(), ct);

            if (exists)
            {
                throw new EntityAlreadyExistsException(nameof(Ingredient));
            }

            return await Create(newIngredient, ct);
        }

        public async Task<ICollection<Ingredient>> GetIngredientsByIds(ICollection<long> ids, CancellationToken ct = default)
        {
            return await _context.Ingredients
                .Where(i => ids.Contains(i.Id))
                .ToListAsync(ct);
        }

        public async Task<ICollection<Ingredient>> SearchIngredientsByName(string name, CancellationToken ct = default)
        {
            return await _context.Ingredients
                .Where(i => i.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync(ct);
        }
    }
}
