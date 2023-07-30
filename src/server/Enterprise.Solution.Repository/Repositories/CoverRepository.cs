using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Enterprise.Solution.Repositories
{
    public class CoverRepository : BaseRepository<Cover>, ICoverRepository
    {
        public CoverRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<Cover>> logger) : base(dbContext, logger) { }

        public async Task<EntityListWithPaginationMetadata<Cover>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeArtists,
            bool includeBook,
            bool includeBookAndAuthor,
            bool onlyShowDeleted)
        {
            if (orderBy == null) orderBy = "title asc";

            var collection = _dbContext.Covers as IQueryable<Cover>;

            collection = collection.Where(c => c.IsDeleted == onlyShowDeleted);

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                throw new NotImplementedException();
            }

            var totalAuthorCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalAuthorCount, pageSize, pageNumber, orderBy);

            if (includeArtists)
            {
                collection = collection
                    .Include(cover => cover.Artists.Where(artist => !artist.IsDeleted));
            }
            if (includeBook)
            {
                collection = collection
                    .Include(cover => cover.Book);
            }
            else if (includeBookAndAuthor)
            {
                collection = collection
                    .Include(cover => cover.Book)
                    .ThenInclude(book => book.Author);
            }

            var collectionToReturn = await collection
                .OrderBy(orderBy)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return new EntityListWithPaginationMetadata<Cover>(collectionToReturn, paginationMetadata);
        }

        public async Task<Cover?> GetByIdAsync(
            int id,
            bool includeArtists,
            bool includeBook,
            bool includeBookAndAuthor)
        {
            var collection = _dbContext.Covers as IQueryable<Cover>;

            if (includeArtists)
            {
                collection = collection
                    .Include(a => a.Artists);
            }
            if (includeBook)
            {
                collection = collection
                    .Include(b => b.Book);
            }
            else if (includeBookAndAuthor)
            {
                collection = collection
                    .Include(b => b.Book)
                    .ThenInclude(c => c.Author);
            }

            return await collection.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }
    }
}
