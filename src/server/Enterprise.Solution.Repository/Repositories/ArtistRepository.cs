using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Enterprise.Solution.Repositories
{
    public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<Artist>> logger) : base(dbContext, logger) { }

        public async Task<EntityListWithPaginationMetadata<Artist>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeCovers,
            bool includeCoversWithBook,
            bool includeCoversWithBookAndAuthor)
        {
            if (orderBy == null) orderBy = "firstName asc, lastName asc";
            
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
            var paginationMetadata = new PaginationMetadata(totalArtistCount, pageSize, pageNumber, orderBy);

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
                .OrderBy(orderBy)
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
            var collection = _dbContext.Artists as IQueryable<Artist>;

            if (includeCovers)
            {
                return await collection
                    .Include(a => a.Covers)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }
            else if (includeCoversWithBook)
            {
                return await collection
                    .Include(a => a.Covers)
                    .ThenInclude(c => c.Book)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }
            else if (includeCoversWithBookAndAuthor)
            {
                return await collection
                    .Include(a => a.Covers)
                    .ThenInclude(c => c.Book)
                    .ThenInclude(b => b.Author)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }

            return await collection.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }
    }
}
