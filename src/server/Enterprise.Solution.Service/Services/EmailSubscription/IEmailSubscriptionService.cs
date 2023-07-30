using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public interface IEmailSubscriptionService : IBaseService<EmailSubscription>
    {
        public Task<EntityListWithPaginationMetadata<EmailSubscription>> ListAllAsync(
            int pageNumber,
            int PageSize,
            string? orderBy,
            string? searchQuery,
            bool onlyShowDeleted);
    }
}
