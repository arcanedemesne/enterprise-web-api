using Enterprise.Solution.Application.Queries;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using MediatR;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for GetEmailSubscriptionByIdHandler
    /// </summary>
    public class GetEmailSubscriptionByIdHandler : IRequestHandler<GetEmailSubscriptionByIdQuery, EmailSubscription>
    {
        private readonly IEmailSubscriptionService _emailSubscriptionService;

        /// <summary>
        /// Constructor for ListAllEmailSubscriptionsHandler
        /// </summary>
        /// <param name="emailSubscriptionService"></param>
        public GetEmailSubscriptionByIdHandler(IEmailSubscriptionService emailSubscriptionService) => _emailSubscriptionService = emailSubscriptionService;

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EmailSubscription> Handle(
            GetEmailSubscriptionByIdQuery request,
            CancellationToken cancellationToken
            ) => await _emailSubscriptionService.GetByIdAsync(request.id);
    }
}
