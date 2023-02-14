using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public interface IItemService : IBaseService<Item>
    {
        public Task<EntityListWithPaginationMetadata<Item>> ListAllAsync(
            string? filter,
            string? search,
            int pageNumber,
            int pageSize
        );
    }
}
