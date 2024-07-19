using ShareSpoon.App.Abstractions;
using ShareSpoon.Domain.Models.BasicEntities;
using ShareSpoon.Infrastructure.Exceptions;

namespace ShareSpoon.Infrastructure.Repositories.BasicRepositories
{
    public abstract class BaseRepositoryInMemory<T> : IBaseRepository<T> where T : BaseEntity
    {
        private static ICollection<T> _entities = new List<T>();

        public async Task<T> Create(T entity, CancellationToken ct = default)
        {
            if (_entities.Contains(entity))
            {
                throw new EntityAlreadyExistsException(typeof(T).Name, entity.Id);
            }
            _entities.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<ICollection<T>> GetAll(CancellationToken ct = default)
        {
            return await Task.FromResult(_entities);
        }

        public async Task<T> GetById(long id, CancellationToken ct = default)
        {
            return await Task.FromResult(_entities.FirstOrDefault(e => e.Id == id)) ?? throw new EntityNotFoundException(typeof(T).Name, id);
        }

        public async Task<T> Update(T updatedEntity, CancellationToken ct = default)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == updatedEntity.Id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(T).Name, updatedEntity.Id);
            }
            entity = updatedEntity;
            return await Task.FromResult(entity);
        }

        public Task Delete(long id, CancellationToken ct = default)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(T).Name, id);
            }
            _entities.Remove(entity);
            return Task.CompletedTask;
        }

        public long GetAvailableId()
        {
            if (_entities.Count == 0)
                return 1;
            return _entities.Max(e => e.Id) + 1;
        }

        public bool EntitiesExist(IEnumerable<long> ids)
        {
            return ids.All(id => _entities.Any(x => x.Id == id));
        }
    }
}
