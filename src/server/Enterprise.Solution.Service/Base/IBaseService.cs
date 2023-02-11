using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;

namespace Enterprise.Solution.Service.Base
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> ListAllAsync();

        Task<(IReadOnlyList<T>, PaginationMetadata)> ListAllAsync(int pageNumber, int pageSize);

        Task<T?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
