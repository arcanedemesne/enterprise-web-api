using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

namespace Enterprise.Solution.Repositories
{
    /// <summary>
    /// Artist Async Repository
    /// </summary>
    /// <typeparam name="Artist"></typeparam>
    public interface IArtistRepository : IBaseRepository<Artist>
    {
        public Task<EntityListWithPaginationMetadata<Artist>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeCovers,
            bool includeCoversWithBook,
            bool includeCoversWithBookAndArtist,
            bool onlyShowDeleted);

        public Task<Artist?> GetByIdAsync(
            int id,
            bool includeCovers,
            bool includeCoversWithBook,
            bool includeCoversWithBookAndArtist);
    }
}
