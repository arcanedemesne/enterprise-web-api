using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repository.Base;

namespace Enterprise.Solution.Repositories
{
    /// <summary>
    /// Cover Async Repository
    /// </summary>
    /// <typeparam name="Cover"></typeparam>
    public interface ICoverRepository : IBaseRepository<Cover>
    {
        public Task<EntityListWithPaginationMetadata<Cover>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeArtists,
            bool includeBook,
            bool includeBookAndAuthor,
            bool onlyShowDeleted);

        public Task<Cover?> GetByIdAsync(
            int id,
            bool includeArtists,
            bool includeBook,
            bool includeBookAndAuthor);
    }
}
