using Microsoft.Extensions.Logging;

using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public class ItemService : BaseService<Item>, IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(
            ILogger<IItemService> logger,
            IBaseRepository<Item> baseRepository,
            IItemRepository itemRepository
        ) : base(logger, baseRepository)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public async Task<EntityListWithPaginationMetadata<Item>> ListAllAsync(
            string? filter,
            string? search,
            int pageNumber,
            int pageSize
        )
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }
            
            return await _itemRepository.ListAllAsync(filter, search, pageNumber, pageSize);
        }
    }
}
