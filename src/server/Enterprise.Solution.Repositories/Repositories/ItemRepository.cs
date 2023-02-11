using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<Item>> logger) : base(dbContext, logger) { }

        public async Task<(IReadOnlyList<Item>, PaginationMetadata)> ListAllAsync(string? filter, string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _dbContext.Items as IQueryable<Item>;

            if (!String.IsNullOrWhiteSpace(filter))
            {
                filter = filter.Trim();
                collection = collection
                .Where(i => i.Name.ToLower().Equals(filter.ToLower()));
            }

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(i => i.Name.ToLower().Contains(searchQuery.ToLower())
                || (i.Description != null && i.Description.ToLower().Contains(searchQuery.ToLower())));
            }

            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                .OrderBy(i => i.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize).ToListAsync();


            return (collectionToReturn, paginationMetadata);
        }
    }
}
