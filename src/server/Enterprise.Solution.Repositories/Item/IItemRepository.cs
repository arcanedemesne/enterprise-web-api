using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;

namespace Enterprise.Solution.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<(IEnumerable<Item>, PaginationMetadata)> GetItemsAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<Item?> GetItemAsync(int itemId);
        Task<bool> ItemExistsAsync(int itemId);
        Task<bool> ItemNameMatchesItemId(string? itemName, int itemId);
        Task<bool> AddItemAsync(Item item);
        Task<bool> UpdateItemAsync(Item item);
        Task DeleteItemAsync(int itemId);
        Task<bool> SaveChangesAsync();
    }
}