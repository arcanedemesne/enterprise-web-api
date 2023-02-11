using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

namespace Enterprise.Solution.Repositories
{
    /// <summary>
    /// Author Async Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        public Task<(IReadOnlyList<Author>, PaginationMetadata)> ListAllAsync(string? filter, string? searchQuery, int pageNumber, int pageSize);
    }
}
