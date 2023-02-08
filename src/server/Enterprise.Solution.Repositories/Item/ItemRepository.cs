using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Solution.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly EnterpriseSolutionDbContext _context;

        public ItemRepository(EnterpriseSolutionDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await _context.Items.OrderBy(i => i.Name).ToListAsync();
        }

        public async Task<(IEnumerable<Item>, PaginationMetadata)> GetItemsAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Items as IQueryable<Item>;

            if (!String.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection
                .Where(i => i.Name.ToLower().Equals(name.ToLower()));
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

        public async Task<Item?> GetItemAsync(int itemId)
        {
            return await _context.Items
                .Where(i => i.Id.Equals(itemId))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ItemExistsAsync(int itemId)
        {
            return await _context.Items.AnyAsync(c => c.Id.Equals(itemId));
        }

        public async Task<bool> ItemNameMatchesItemId(string? itemName, int itemId)
        {
            return await _context.Items.AnyAsync(c => c.Id.Equals(itemId) && c.Name.Equals(itemName));
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            _context.Items.Add(item);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return await SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                _context.Items.Remove(item);
                await SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
