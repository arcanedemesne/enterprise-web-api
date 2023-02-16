using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Repositories
{
    public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<Artist>> logger) : base(dbContext, logger) { }

        public async Task<EntityListWithPaginationMetadata<Artist>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? searchQuery,
            bool includeCovers,
            bool includeCoversWithBook,
            bool includeCoversWithBookAndAuthor)
        {
            var collection = _dbContext.Artists as IQueryable<Artist>;

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a =>
                a.FirstName.ToLower().Contains(searchQuery.ToLower())
                || a.LastName.ToLower().Contains(searchQuery.ToLower()));
            }

            var totalArtistCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalArtistCount, pageSize, pageNumber);

            if (includeCovers)
            {
                collection = collection
                    .Include(a => a.Covers);
            }
            else if (includeCoversWithBook)
            {
                collection = collection
                    .Include(a => a.Covers)
                    .ThenInclude(c => c.Book);
            }
            else if (includeCoversWithBookAndAuthor)
            {
                collection = collection
                    .Include(a => a.Covers)
                    .ThenInclude(b => b.Book)
                    .ThenInclude(c => c.Author);
            }

            var collectionToReturn = await collection
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return new EntityListWithPaginationMetadata<Artist>(collectionToReturn, paginationMetadata);
        }

        public async Task<Artist?> GetByIdAsync(
            int id,
            bool includeCovers,
            bool includeCoversWithBook,
            bool includeCoversWithBookAndAuthor)
        {
            if (includeCovers)
            {
                return await _dbContext.Artists
                    .Include(a => a.Covers)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }
            else if (includeCoversWithBook)
            {
                return await _dbContext.Artists
                    .Include(a => a.Covers)
                    .ThenInclude(c => c.Book)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }
            else if (includeCoversWithBookAndAuthor)
            {
                return await _dbContext.Artists
                    .Include(a => a.Covers)
                    .ThenInclude(c => c.Book)
                    .ThenInclude(b => b.Author)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }

            return await _dbContext.Artists.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }
    }
}
