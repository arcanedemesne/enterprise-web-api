using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

namespace Enterprise.Solution.Repositories
{
    /// <summary>
    /// IItem Async Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IItemRepository : IBaseRepository<Item>
    {
        Task<EntityListWithPaginationMetadata<Item>> ListAllAsync(string? filter, string? searchQuery, int pageNumber, int pageSize);
    }
}
