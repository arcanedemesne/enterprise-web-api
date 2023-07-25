using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models.Base;
using System.Linq.Dynamic.Core;

namespace Enterprise.Solution.Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        public readonly EnterpriseSolutionDbContext _dbContext;
        private readonly ILogger<BaseRepository<T>> _logger;

        public BaseRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<T>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public virtual async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<EntityListWithPaginationMetadata<T>> ListAllAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? orderBy = null)
        {
            var collection = _dbContext.Set<T>() as IQueryable<T>;

            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                .OrderBy(orderBy ?? "Id ASC")
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return new EntityListWithPaginationMetadata<T>(collectionToReturn, paginationMetadata);
        }

        public virtual Task<EntityListWithPaginationMetadata<T>> ListAllAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? orderBy = null,
            string? searchQuery = null)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Set<T>().AnyAsync(c => c.Id.Equals(id));
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(a => a.Id == id);
        }

        //TODO: Determine how to do Bulk Import and Bulk Update/Patches

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {

            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
