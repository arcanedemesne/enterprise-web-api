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
    public class ListAllArtistsHandler : BaseHandler<ListAllArtistsHandler>, IRequestHandler<ListAllArtistsQuery, IReadOnlyList<Artist>>
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
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<Artist>> Handle(ListAllArtistsQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<ListAllArtistsQuery>();

            LogTryServiceRequest<Artist>(RequestType.ListAll);
            return await _service.ListAllAsync(
                request.QueryParams.SearchQuery,
                request.QueryParams.OnlyShowDeleted ?? false);
        }
    }
}
