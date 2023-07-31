using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Service.Base
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> ListAllAsync(string? searchQuery, bool onlyShowDeleted);

        Task<EntityListWithPaginationMetadata<T>> ListPagedAsync(int pageNumber, int pageSize, string orderBy, string? searchQuery, bool onlyShowDeleted);

        Task<T?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
