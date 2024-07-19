using Microsoft.EntityFrameworkCore;
using ShareSpoon.App.Abstractions;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Recipes;
using ShareSpoon.Infrastructure.Exceptions;
using ShareSpoon.Infrastructure.Repositories.BasicRepositories;

namespace ShareSpoon.Infrastructure.Repositories
{
    public class TagRepository : BaseRepositoryEF<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Tag> CreateTag(Tag newTag, CancellationToken ct = default)
        {
            var exists = await _context.Tags
                .AnyAsync(t => t.Name.ToLower() == newTag.Name.ToLower(), ct);

            if (exists)
            {
                throw new EntityAlreadyExistsException(nameof(Tag));
            }

            return await Create(newTag, ct);
        }

        public async Task<ICollection<Tag>> GetTagsByIds(ICollection<long> ids, CancellationToken ct = default)
        {
            return await _context.Tags
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(ct);
        }

        public async Task<ICollection<Tag>> SearchTagsByName(string name, CancellationToken ct = default)
        {
            return await _context.Tags
                .Where(t => t.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync(ct);
        }

        public async Task<ICollection<Tag>> GetFilterTags(CancellationToken ct = default)
        {
            return await _context.Tags
                .Where(t => t.Type == TagType.Course || t.Type == TagType.DietaryPreference)
                .ToListAsync(ct);
        }
    }
}
