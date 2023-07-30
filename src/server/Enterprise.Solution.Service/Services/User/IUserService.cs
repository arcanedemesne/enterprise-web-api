using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public interface IUserService : IBaseService<User>
    {
        public Task<EntityListWithPaginationMetadata<User>> ListAllAsync(
            int pageNumber,
            int PageSize,
            string? orderBy,
            string? searchQuery,
            bool onlyShowDeleted);
        public Task<bool> ExistsAsync(Guid id);
        public Task<User?> GetByIdAsync(Guid id);
    }
}
