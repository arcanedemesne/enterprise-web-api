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
    public class ListPagedArtistsHandler : BaseHandler<ListPagedArtistsHandler>, IRequestHandler<ListPagedArtistsQuery, EntityListWithPaginationMetadata<Artist>>
    {
        private readonly IArtistService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public ListPagedArtistsHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<ListPagedArtistsHandler> logger,
            IMapper mapper,
            IArtistService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EntityListWithPaginationMetadata<Artist>> Handle(ListPagedArtistsQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<ListPagedArtistsQuery>();

            var (pageNumber, pageSize) = ValidatePagedParams(request.QueryParams.PageNumber, request.QueryParams.PageSize);

            LogTryServiceRequest<Artist>(RequestType.ListPaged);
            return await _service.ListPagedAsync(
                pageNumber, pageSize,
                request.QueryParams.OrderBy,
                request.QueryParams.SearchQuery,
                request.QueryParams.IncludeCovers ?? false,
                request.QueryParams.IncludeCoversWithBook ?? false,
                request.QueryParams.IncludeCoversWithBookAndAuthor ?? false,
                request.QueryParams.OnlyShowDeleted ?? false);
        }
    }
}
