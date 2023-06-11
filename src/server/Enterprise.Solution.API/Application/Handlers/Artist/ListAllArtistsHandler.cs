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
    public class ListAllArtistsHandler : BaseHandler<ListAllArtistsHandler>, IRequestHandler<ListAllArtistsQuery, EntityListWithPaginationMetadata<Artist>>
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
        public ListAllArtistsHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<ListAllArtistsHandler> logger,
            IMapper mapper,
            IArtistService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(ListAllArtistsHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EntityListWithPaginationMetadata<Artist>> Handle(ListAllArtistsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ListAllArtistsHandler)} handler invoked with request type {nameof(ListAllArtistsQuery)}.");

            var (pageNumber, pageSize) = ValidatePagedParams(request.QueryParams.PageNumber, request.QueryParams.PageSize);

            _logger.LogInformation($"Try to list all of type {nameof(Artist)}");
            return await _service.ListAllAsync(
                pageNumber,
                pageSize,
                request.QueryParams.SearchQuery,
                request.QueryParams.IncludeCovers ?? false,
                request.QueryParams.IncludeCoversWithBook ?? false,
                request.QueryParams.IncludeCoversWithBookAndAuthor ?? false);
        }
    }
}
