using Microsoft.EntityFrameworkCore;
using ShareSpoon.App.Abstractions;
using ShareSpoon.Domain.Models.BasicEntities;
using ShareSpoon.Infrastructure.Exceptions;

namespace ShareSpoon.Infrastructure.Repositories.BasicRepositories
{
    public class BaseRepositoryEF<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;

        public BaseRepositoryEF(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> Create(T entity, CancellationToken ct = default)
        {
            if (_context.Set<T>().Contains(entity))
            {
                throw new EntityAlreadyExistsException(typeof(T).Name, entity.Id);
            }
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<ICollection<T>> GetAll(CancellationToken ct = default)
        {
            return await _context.Set<T>().ToListAsync(ct);
        }

        public async Task<T> GetById(long id, CancellationToken ct = default)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id, ct) ?? throw new EntityNotFoundException(typeof(T).Name, id);
        }

        public async Task<T> Update(T updatedEntity, CancellationToken ct = default)
        {
            var entityExists = _context.Set<T>().Any(e => e.Id == updatedEntity.Id);
            if (!entityExists)
            {
                throw new EntityNotFoundException(typeof(T).Name, updatedEntity.Id);
            }
            _context.Set<T>().Update(updatedEntity);
            await _context.SaveChangesAsync(ct);
            return updatedEntity;
        }

        public async Task Delete(long id, CancellationToken ct = default)
        {
            var entityToDelete = _context.Set<T>().FirstOrDefault(e => e.Id == id);
            if (entityToDelete == null)
            {
                throw new EntityNotFoundException(typeof(T).Name, id);
            }
            _context.Set<T>().Remove(entityToDelete);
            await _context.SaveChangesAsync(ct);
        }

        public bool EntitiesExist(IEnumerable<long> ids)
        {
            return ids.All(id => _context.Set<T>().Any(x => x.Id == id));
        }
    }
}
