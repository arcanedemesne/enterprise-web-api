using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Base;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class EmailSubscriptionService : BaseService<EmailSubscription>, IEmailSubscriptionService
    {
        private readonly IEmailSubscriptionRepository _emailSubscriptionRepository;

        public EmailSubscriptionService(
            ILogger<IEmailSubscriptionService> logger,
            IBaseRepository<EmailSubscription> baseRepository,
            IEmailSubscriptionRepository emailSubscriptionRepository
        ) : base(logger, baseRepository)
        {
            _emailSubscriptionRepository = emailSubscriptionRepository ?? throw new ArgumentNullException(nameof(emailSubscriptionRepository));
        }

        public async override Task<IReadOnlyList<EmailSubscription>> ListAllAsync(
            string? searchQuery = null,
            bool onlyShowDeleted = false)
        {
            return await _emailSubscriptionRepository.ListAllAsync(
                searchQuery,
                onlyShowDeleted);
        }

        public async override Task<EntityListWithPaginationMetadata<EmailSubscription>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery = null,
            bool onlyShowDeleted = false)
        {
            return await _emailSubscriptionRepository.ListPagedAsync(
                pageNumber,
                pageSize,
                orderBy,
                searchQuery,
                onlyShowDeleted);
        }
    }
}
