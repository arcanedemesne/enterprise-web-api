using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Repositories
{
    public class EmailSubscriptionRepository : BaseRepository<EmailSubscription>, IEmailSubscriptionRepository
    {
        public EmailSubscriptionRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<EmailSubscription>> logger) : base(dbContext, logger) { }

        public async Task<EntityListWithPaginationMetadata<EmailSubscription>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? searchQuery)
        {
            var collection = _dbContext.EmailSubscriptions as IQueryable<EmailSubscription>;

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a =>
                a.FirstName.ToLower().Contains(searchQuery.ToLower())
                || a.LastName.ToLower().Contains(searchQuery.ToLower())
                || a.EmailAddress.ToLower().Contains(searchQuery.ToLower()));
            }

            var totalAuthorCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalAuthorCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return new EntityListWithPaginationMetadata<EmailSubscription>(collectionToReturn, paginationMetadata);
        }
    }
}
