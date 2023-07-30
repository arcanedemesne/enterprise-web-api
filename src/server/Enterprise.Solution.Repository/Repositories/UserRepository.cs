using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Enterprise.Solution.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<User>> logger) : base(dbContext, logger) { }

        public async override Task<EntityListWithPaginationMetadata<User>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool onlyShowDeleted)
        {
            if (orderBy == null) orderBy = "firstName asc, lastName asc";

            var collection = _dbContext.Users as IQueryable<User>;

            collection = collection.Where(c => c.IsDeleted == onlyShowDeleted);

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a =>
                a.FirstName.ToLower().Contains(searchQuery.ToLower())
                || a.LastName.ToLower().Contains(searchQuery.ToLower())
                || a.EmailAddress.ToLower().Contains(searchQuery.ToLower())
                || a.UserName.ToLower().Contains(searchQuery.ToLower()));
            }

            var totalAuthorCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalAuthorCount, pageSize, pageNumber, orderBy);

            var collectionToReturn = await collection
                .OrderBy(orderBy)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return new EntityListWithPaginationMetadata<User>(collectionToReturn, paginationMetadata);
        }

        public virtual async Task<bool> ExistsAsync(Guid uid)
        {
            return await _dbContext.Users.AnyAsync(c => c.KeycloakUniqueIdentifier.Equals(uid));
        }

        public async Task<User?> GetByIdAsync(Guid uid)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(a => a.KeycloakUniqueIdentifier.Equals(uid));
        }
    }
}
