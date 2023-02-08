using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;

namespace Enterprise.Solution.Service.Services
{
    public interface IItemService
    {
        public Task<(IEnumerable<Item>, PaginationMetadata)> GetItemsAsync(
            string? name,
            string? searchQuery,
            int pageNumber = 1,
            int pageSize = 10);

        public Task<Item?> GetItemAsync(int itemId);
        public Task<bool> ItemExistsAsync(int itemId);

        public Task<bool> AddItemAsync(Item item);
        public Task<bool> UpdateItemAsync(Item item);
        public Task DeleteItemAsync(int itemId);
    }
}
