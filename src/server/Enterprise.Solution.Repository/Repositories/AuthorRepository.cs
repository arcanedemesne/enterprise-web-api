using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Entities;
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
            string? filter,
            string? searchQuery,
            int pageNumber,
            int pageSize,
            bool includeBooks
        )
        {
            var collection = _dbContext.Authors as IQueryable<Author>;

            if (!String.IsNullOrWhiteSpace(filter))
            {
                filter = filter.Trim();
                collection = collection
                .Where(a =>
                a.FirstName.ToLower().Equals(filter.ToLower()));
            }

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

        public async Task<Author?> GetByIdAsync(int id, bool includeBooks)
        {
            if (includeBooks)
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
