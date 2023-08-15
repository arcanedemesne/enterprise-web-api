using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repository.Base;

namespace Enterprise.Solution.Repositories
{
    /// <summary>
    /// Book Async Repository
    /// </summary>
    /// <typeparam name="Book"></typeparam>
    public interface IBookRepository : IBaseRepository<Book>
    {
        public Task<EntityListWithPaginationMetadata<Book>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeAuthor,
            bool includeCover,
            bool includeCoverAndArtists,
            bool onlyShowDeleted);

        public Task<Book?> GetByIdAsync(
            int id,
            bool includeAuthor,
            bool includeCover,
            bool includeCoverAndArtists);
    }
}
