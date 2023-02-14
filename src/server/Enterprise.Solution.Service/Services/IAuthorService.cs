using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public interface IAuthorService : IBaseService<Author>
    {
        public Task<EntityListWithPaginationMetadata<Author>> ListAllAsync(
            string? filter,
            string? searchQuery,
            int pageNumber,
            int pageSize,
            bool includeBooks
        );

        public Task<Author?> GetByIdAsync(int id, bool includeBooks);
    }
}
