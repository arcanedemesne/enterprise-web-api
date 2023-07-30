using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

namespace Enterprise.Solution.Repositories
{
    /// <summary>
    /// Author Async Repository
    /// </summary>
    /// <typeparam name="Author"></typeparam>
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        public Task<EntityListWithPaginationMetadata<Author>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeBooks,
            bool includeBooksWithCover,
            bool includeBooksWithCoverAndArtists,
            bool onlyShowDeleted);

        public Task<Author?> GetByIdAsync(
            int id,
            bool includeBooks,
            bool includeBooksWithCover,
            bool includeBooksWithCoverAndArtists);
    }
}
