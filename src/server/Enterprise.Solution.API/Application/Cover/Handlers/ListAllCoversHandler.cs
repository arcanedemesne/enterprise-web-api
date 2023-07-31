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
    /// Handler for ListAll Query
    /// </summary>
    public class ListAllCoversHandler : BaseHandler<ListAllCoversHandler>, IRequestHandler<ListAllCoversQuery, IReadOnlyList<Cover>>
    {
        private readonly ICoverService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public ListAllCoversHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<ListAllCoversHandler> logger,
            IMapper mapper,
            ICoverService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<Cover>> Handle(ListAllCoversQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<ListAllCoversQuery>();

            LogTryServiceRequest<Cover>(RequestType.ListAll);
            return await _service.ListAllAsync(
                request.QueryParams.SearchQuery,
                request.QueryParams.OnlyShowDeleted ?? false);
        }
    }
}
