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

        public async Task<EntityListWithPaginationMetadata<EmailSubscription>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery = null)
        {
            return await _emailSubscriptionRepository.ListAllAsync(
                pageNumber,
                pageSize,
                orderBy,
                searchQuery);
        }
    }
}
