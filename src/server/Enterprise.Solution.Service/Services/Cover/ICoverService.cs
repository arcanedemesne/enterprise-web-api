using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public interface ICoverService : IBaseService<Cover>
    {
        public Task<EntityListWithPaginationMetadata<Cover>> ListAllAsync(
            int pageNumber,
            int PageSize,
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
