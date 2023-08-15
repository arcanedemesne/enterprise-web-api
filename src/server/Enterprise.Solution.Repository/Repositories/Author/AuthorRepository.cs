using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;


namespace Enterprise.Solution.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<Author>> logger) : base(dbContext, logger) { }

        public async override Task<IReadOnlyList<Author>> ListAllAsync(
            string? searchQuery,
            bool onlyShowDeleted)
        {
            var collection = _dbContext.Authors as IQueryable<Author>;

            collection = collection.Where(c => c.IsDeleted == onlyShowDeleted);

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a =>
                a.FirstName.ToLower().Contains(searchQuery.ToLower())
                || a.LastName.ToLower().Contains(searchQuery.ToLower()));
            }

            return await collection.ToListAsync();
        }

        public async Task<EntityListWithPaginationMetadata<Author>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeBooks,
            bool includeBooksWithCover,
            bool includeBooksWithCoverAndArtists,
            bool onlyShowDeleted)
        {
            if (orderBy == null) orderBy = "firstName asc, lastName asc";

            var collection = _dbContext.Authors as IQueryable<Author>;

            collection = collection.Where(c => c.IsDeleted == onlyShowDeleted);

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a =>
                a.FirstName.ToLower().Contains(searchQuery.ToLower())
                || a.LastName.ToLower().Contains(searchQuery.ToLower()));
            }

            var totalAuthorCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalAuthorCount, pageSize, pageNumber, orderBy);

            if (includeBooks)
            {
                collection = collection
                    .Include(author => author.Books.Where(book => !book.IsDeleted));
            }
            else if (includeBooksWithCover)
            {
                collection = collection
                    .Include(author => author.Books.Where(book => !book.IsDeleted))
                    .ThenInclude(book => book.Cover);
            }
            else if (includeBooksWithCoverAndArtists)
            {
                collection = collection
                    .Include(author => author.Books.Where(book => !book.IsDeleted))
                    .ThenInclude(book => book.Cover)
                    .ThenInclude(cover => cover.Artists.Where(artist => !artist.IsDeleted));
            }

            var collectionToReturn = await collection
                .OrderBy(orderBy)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return new EntityListWithPaginationMetadata<Author>(collectionToReturn, paginationMetadata);
        }

        public async Task<Author?> GetByIdAsync(
            int id,
            bool includeBooks,
            bool includeBooksWithCover,
            bool includeBooksWithCoverAndArtists)
        {
            var collection = _dbContext.Authors as IQueryable<Author>;

            if (includeBooks)
            {
                return await collection
                    .Include(a => a.Books)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }
            else if (includeBooksWithCover)
            {
                return await collection
                    .Include(a => a.Books)
                    .ThenInclude(b => b.Cover)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }
            else if (includeBooksWithCoverAndArtists)
            {
                return await collection
                    .Include(a => a.Books)
                    .ThenInclude(b => b.Cover)
                    .ThenInclude(c => c.Artists)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }

            return await collection.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }
    }
}
