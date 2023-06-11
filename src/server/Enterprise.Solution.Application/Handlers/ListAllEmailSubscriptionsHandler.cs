using Enterprise.Solution.Application.Queries;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using MediatR;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for ListAllEmailSubscriptionsHandler
    /// </summary>
    public class ListAllEmailSubscriptionsHandler : IRequestHandler<ListAllEmailSubscriptionsQuery, EntityListWithPaginationMetadata<EmailSubscription>>
    {
        private readonly IEmailSubscriptionService _emailSubscriptionService;

        /// <summary>
        /// Constructor for ListAllEmailSubscriptionsHandler
        /// </summary>
        /// <param name="emailSubscriptionService"></param>
        public ListAllEmailSubscriptionsHandler(IEmailSubscriptionService emailSubscriptionService) => _emailSubscriptionService = emailSubscriptionService;

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EntityListWithPaginationMetadata<EmailSubscription>> Handle(
            ListAllEmailSubscriptionsQuery request,
            CancellationToken cancellationToken
            ) => await _emailSubscriptionService.ListAllAsync(request.queryParams.PageNumber, request.queryParams.PageSize, request.queryParams.SearchQuery);
    }
}
