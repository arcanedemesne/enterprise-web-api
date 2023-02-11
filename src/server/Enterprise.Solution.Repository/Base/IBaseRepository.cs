using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;

namespace Enterprise.Solution.Repository.Base
{
    /// <summary>
    /// Templated Async Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : BaseEntity
    {

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<(IReadOnlyList<T>, PaginationMetadata)> ListAllAsync(int pageNumber, int pageSize);

        //Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<T?> GetByIdAsync(int id, List<string>? include = null!);

        Task<bool> ExistsAsync(int id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        //Task<int> CountAsync(ISpecification<T> spec);
    }
}
