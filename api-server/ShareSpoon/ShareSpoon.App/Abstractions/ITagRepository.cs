using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.App.Abstractions
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Task<Tag> CreateTag(Tag newTag, CancellationToken ct = default);
        Task<ICollection<Tag>> GetTagsByIds(ICollection<long> ids, CancellationToken ct = default);
        Task<ICollection<Tag>> SearchTagsByName(string name, CancellationToken ct = default);
        Task<ICollection<Tag>> GetFilterTags(CancellationToken ct = default);
    }
}
