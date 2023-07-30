using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Repository.Base
{
    /// <summary>
    /// Templated Async Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> ListAllAsync();

        Task<EntityListWithPaginationMetadata<T>> ListAllAsync(int pageNumber, int pageSize, string? orderBy, bool onlyShowDeleted);
        Task<EntityListWithPaginationMetadata<T>> ListAllAsync(int pageNumber, int pageSize, string? orderBy, string? searchQuery, bool onlyShowDeleted);

        //Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<T?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        //Task<int> CountAsync(ISpecification<T> spec);
    }
}
