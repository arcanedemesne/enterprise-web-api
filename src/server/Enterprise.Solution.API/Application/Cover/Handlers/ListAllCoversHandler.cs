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
    public class ListAllCoversHandler : BaseHandler<ListAllCoversHandler>, IRequestHandler<ListAllCoversQuery, EntityListWithPaginationMetadata<Cover>>
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
        public async Task<EntityListWithPaginationMetadata<Cover>> Handle(ListAllCoversQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<ListAllCoversQuery>();

            var (pageNumber, pageSize) = ValidatePagedParams(request.QueryParams.PageNumber, request.QueryParams.PageSize);

            LogTryServiceRequest<Cover>(RequestType.ListAll);
            return await _service.ListAllAsync(
                pageNumber, pageSize,
                request.QueryParams.OrderBy,
                request.QueryParams.SearchQuery,
                request.QueryParams.IncludeAuthor ?? false,
                request.QueryParams.IncludeCover ?? false,
                request.QueryParams.IncludeCoverAndArtists ?? false);
        }
    }
}
