using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Base;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class ArtistService : BaseService<Artist>, IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(
            ILogger<IArtistService> logger,
            IBaseRepository<Artist> baseRepository,
            IArtistRepository artistRepository
        ) : base(logger, baseRepository)
        {
            _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
        }

        public async Task<EntityListWithPaginationMetadata<Artist>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeCovers,
            bool includeCoversWithBook,
            bool includeCoversWithBookAndAuthor,
            bool onlyShowDeleted)
        {
            return await _artistRepository.ListPagedAsync(
                pageNumber,
                pageSize,
                orderBy,
                searchQuery,
                includeCovers,
                includeCoversWithBook,
                includeCoversWithBookAndAuthor,
                onlyShowDeleted);
        }

        public async Task<Artist?> GetByIdAsync(
            int id,
            bool includeCovers,
            bool includeCoversWithBook,
            bool includeCoversWithBookAndAuthor)
        {
            return await _artistRepository.GetByIdAsync(
                id,
                includeCovers,
                includeCoversWithBook,
                includeCoversWithBookAndAuthor);
        }
    }
}
