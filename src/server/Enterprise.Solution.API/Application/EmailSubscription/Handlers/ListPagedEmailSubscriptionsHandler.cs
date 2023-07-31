using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for ListPaged Query
    /// </summary>
    public class ListPagedEmailSubscriptionsHandler : BaseHandler<ListPagedEmailSubscriptionsHandler>, IRequestHandler<ListPagedEmailSubscriptionsQuery, EntityListWithPaginationMetadata<EmailSubscription>>
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
        public ListPagedEmailSubscriptionsHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<ListPagedEmailSubscriptionsHandler> logger,
            IMapper mapper,
            IEmailSubscriptionService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EntityListWithPaginationMetadata<EmailSubscription>> Handle(ListPagedEmailSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<ListPagedEmailSubscriptionsQuery>();

            var (pageNumber, pageSize) = ValidatePagedParams(request.QueryParams.PageNumber, request.QueryParams.PageSize);

            LogTryServiceRequest<EmailSubscription>(RequestType.ListPaged);
            return await _service.ListPagedAsync(
                pageNumber, pageSize,
                request.QueryParams.OrderBy ?? "",
                request.QueryParams.SearchQuery,
                request.QueryParams.OnlyShowDeleted ?? false);
        }
    }
}
