using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Base;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class ItemService : BaseService<Item>, IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(
            ILogger<IItemService> logger,
            IBaseRepository<Item> baseRepository,
            IItemRepository itemRepository
        ) : base(logger, baseRepository) {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public async Task<(IReadOnlyList<Item>, PaginationMetadata)> ListAllAsync(
            string? filter,
            string? search,
            int pageNumber,
            int pageSize
        )
        {
            if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }

            return await _itemRepository.ListAllAsync(filter, search, pageNumber, pageSize);
        }
    }
}
