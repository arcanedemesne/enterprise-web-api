﻿using AutoMapper;
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
    /// Handler for ListAll Query
    /// </summary>
    public class ListAllEmailSubscriptionsHandler : BaseHandler<ListAllEmailSubscriptionsHandler>, IRequestHandler<ListAllEmailSubscriptionsQuery, IReadOnlyList<EmailSubscription>>
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
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<EmailSubscription>> Handle(ListAllEmailSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<ListAllEmailSubscriptionsQuery>();

            LogTryServiceRequest<EmailSubscription>(RequestType.ListAll);
            return await _service.ListAllAsync(
                request.QueryParams.SearchQuery,
                request.QueryParams.OnlyShowDeleted ?? false);
        }
    }
}
