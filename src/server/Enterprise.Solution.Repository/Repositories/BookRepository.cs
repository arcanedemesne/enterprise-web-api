using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Enterprise.Solution.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<Book>> logger) : base(dbContext, logger) { }


        public async override Task<IReadOnlyList<Book>> ListAllAsync(
            string? searchQuery,
            bool onlyShowDeleted)
        {
            var collection = _dbContext.Books as IQueryable<Book>;

            collection = collection.Where(c => c.IsDeleted == onlyShowDeleted);

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a => a.Title.ToLower().Contains(searchQuery.ToLower()));
            }

            return await collection.ToListAsync();
        }
        public async Task<EntityListWithPaginationMetadata<Book>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeAuthor,
            bool includeCover,
            bool includeCoverAndArtists,
            bool onlyShowDeleted)
        {
            if (orderBy == null) orderBy = "title asc";

            var collection = _dbContext.Books as IQueryable<Book>;

            collection = collection.Where(c => c.IsDeleted == onlyShowDeleted);

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a => a.Title.ToLower().Contains(searchQuery.ToLower()));
            }

            var totalAuthorCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalAuthorCount, pageSize, pageNumber, orderBy);

            if (includeAuthor)
            {
                collection = collection
                    .Include(book => book.Author);
            }
            if (includeCover)
            {
                collection = collection
                    .Include(book => book.Cover);
            }
            else if (includeCoverAndArtists)
            {
                collection = collection
                    .Include(book => book.Cover)
                    .ThenInclude(cover => cover.Artists.Where(artist => !artist.IsDeleted));
            }

            var collectionToReturn = await collection
                .OrderBy(orderBy)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return new EntityListWithPaginationMetadata<Book>(collectionToReturn, paginationMetadata);
        }

        public async Task<Book?> GetByIdAsync(
            int id,
            bool includeAuthor,
            bool includeCover,
            bool includeCoverAndArtists)
        {
            var collection = _dbContext.Books as IQueryable<Book>;

            if (includeAuthor)
            {
                collection = collection
                    .Include(b => b.Author);
            }
            if (includeCover)
            {
                return await collection
                    .Include(b => b.Cover)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }
            else if (includeCoverAndArtists)
            {
                return await collection
                    .Include(b => b.Cover)
                    .ThenInclude(c => c.Artists)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }

            return await collection.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }
    }
}
