using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models.Base;
using Enterprise.Solution.Repository.Base;

using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Base
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly ILogger<IBaseService<T>> _logger;
        protected readonly IBaseRepository<T> _repository;

        protected readonly int MaxPageSize = 20;

        public BaseService(
            ILogger<IBaseService<T>> logger,
            IBaseRepository<T> baseRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _repository.ListAllAsync();
        }
        public async Task<EntityListWithPaginationMetadata<T>> ListAllAsync(int pageNumber, int pageSize, string orderBy)
        {
            return await _repository.ListAllAsync(pageNumber, pageSize, orderBy);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
