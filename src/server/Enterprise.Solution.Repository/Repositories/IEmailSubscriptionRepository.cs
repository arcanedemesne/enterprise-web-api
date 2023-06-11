using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

namespace Enterprise.Solution.Repositories
{
    /// <summary>
    /// EmailSubscription Async Repository
    /// </summary>
    /// <typeparam name="EmailSubscription"></typeparam>
    public interface IEmailSubscriptionRepository : IBaseRepository<EmailSubscription>
    {
        public Task<EntityListWithPaginationMetadata<EmailSubscription>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? searchQuery);
    }
}
