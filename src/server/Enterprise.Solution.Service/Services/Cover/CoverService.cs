using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Base;

using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class CoverService : BaseService<Cover>, ICoverService
    {
        private readonly ICoverRepository _coverRepository;

        public CoverService(
            ILogger<ICoverService> logger,
            IBaseRepository<Cover> baseRepository,
            ICoverRepository coverRepository
        ) : base(logger, baseRepository)
        {
            _coverRepository = coverRepository ?? throw new ArgumentNullException(nameof(coverRepository));
        }

        public async Task<EntityListWithPaginationMetadata<Cover>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeArtists,
            bool includeBook,
            bool includeBookAndAuthor,
            bool onlyShowDeleted)
        {
            return await _coverRepository.ListPagedAsync(
                pageNumber,
                pageSize,
                orderBy,
                searchQuery,
                includeArtists,
                includeBook,
                includeBookAndAuthor,
                onlyShowDeleted);
        }

        public async Task<Cover?> GetByIdAsync(
            int id,
            bool includeArtists,
            bool includeBook,
            bool includeBookAndAuthor)
        {
            return await _coverRepository.GetByIdAsync(
                id,
                includeArtists,
                includeBook,
                includeBookAndAuthor);
        }
    }
}
