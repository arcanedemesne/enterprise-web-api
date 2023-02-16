using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<Author>> logger) : base(dbContext, logger) { }

        public async Task<EntityListWithPaginationMetadata<Author>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? searchQuery,
            bool includeBooks,
            bool includeBooksWithCover,
            bool includeBooksWithCoverAndArtists)
        {
            var collection = _dbContext.Authors as IQueryable<Author>;

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a =>
                a.FirstName.ToLower().Contains(searchQuery.ToLower())
                || a.LastName.ToLower().Contains(searchQuery.ToLower()));
            }

            var totalAuthorCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalAuthorCount, pageSize, pageNumber);

            if (includeBooks)
            {
                collection = collection
                    .Include(a => a.Books);
            }
            else if (includeBooksWithCover)
            {
                collection = collection
                    .Include(a => a.Books)
                    .ThenInclude(b => b.Cover);
            }
            else if (includeBooksWithCoverAndArtists)
            {
                collection = collection
                    .Include(a => a.Books)
                    .ThenInclude(b => b.Cover)
                    .ThenInclude(c => c.Artists);
            }

            var collectionToReturn = await collection
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
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
            if (includeBooks)
            {
                return await _dbContext.Authors
                    .Include(a => a.Books)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }
            else if (includeBooksWithCover)
            {
                return await _dbContext.Authors
                    .Include(a => a.Books)
                    .ThenInclude(b => b.Cover)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }
            else if (includeBooksWithCoverAndArtists)
            {
                return await _dbContext.Authors
                    .Include(a => a.Books)
                    .ThenInclude(b => b.Cover)
                    .ThenInclude(c => c.Artists)
                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
            }

            return await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }
    }
}
