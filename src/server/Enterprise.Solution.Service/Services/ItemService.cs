using AutoMapper;
using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repositories;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class ItemService : IItemService
    {
        private readonly ILogger<ItemService> _logger;
        private readonly IItemRepository _itemRepository;
        private readonly int maxPageSize = 20;

        public ItemService(ILogger<ItemService> logger, IItemRepository itemRepository)
        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public async Task<(IEnumerable<Item>, PaginationMetadata)> GetItemsAsync(
            string? name,
            string? searchQuery,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            return await _itemRepository.GetItemsAsync(name, searchQuery, pageNumber, pageSize);
        }

        public async Task<Item?> GetItemAsync(int itemId)
        {
            return await _itemRepository.GetItemAsync(itemId);
        }

        public async Task<bool> ItemExistsAsync(int intemId)
        {
            return await _itemRepository.ItemExistsAsync(intemId);
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            return await _itemRepository.AddItemAsync(item);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            return await _itemRepository.UpdateItemAsync(item);
        }

        public async Task DeleteItemAsync(int itemId)
        {
            await _itemRepository.DeleteItemAsync(itemId);
        }
    }
}
