using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for ListAll Query
    /// </summary>
    public class ListAllEmailSubscriptionsHandler : BaseHandler<ListAllEmailSubscriptionsHandler>, IRequestHandler<ListAllEmailSubscriptionsQuery, EntityListWithPaginationMetadata<EmailSubscription>>
    {
        private readonly IEmailSubscriptionService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public ListAllEmailSubscriptionsHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<ListAllEmailSubscriptionsHandler> logger,
            IMapper mapper,
            IEmailSubscriptionService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(ListAllEmailSubscriptionsHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EntityListWithPaginationMetadata<EmailSubscription>> Handle(ListAllEmailSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ListAllEmailSubscriptionsHandler)} handler invoked with request type {nameof(ListAllEmailSubscriptionsQuery)}.");

            var (pageNumber, pageSize) = ValidatePagedParams(request.QueryParams.PageNumber, request.QueryParams.PageSize);

            _logger.LogInformation($"Try to list all of type {nameof(EmailSubscription)}");
            return await _service.ListAllAsync(pageNumber, pageSize, request.QueryParams.SearchQuery);
        }
    }
}
