using ShareSpoon.Domain.Models.BasicEntities;

namespace ShareSpoon.App.Abstractions
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> Create(T entity, CancellationToken ct = default);
        Task<ICollection<T>> GetAll(CancellationToken ct = default);
        Task<T> GetById(long id, CancellationToken ct = default);
        Task<T> Update(T entity, CancellationToken ct = default);
        Task Delete(long id, CancellationToken ct = default);
        bool EntitiesExist(IEnumerable<long> ids);
    }
}
