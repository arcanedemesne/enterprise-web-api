using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Get By Id Query
    /// </summary>
    public class GetCoverByIdHandler : BaseHandler<GetCoverByIdHandler>, IRequestHandler<GetCoverByIdQuery, CoverDTO_Response>
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
        public GetCoverByIdHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<GetCoverByIdHandler> logger,
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
        public async Task<CoverDTO_Response> Handle(GetCoverByIdQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<GetCoverByIdQuery>();

            LogTryServiceRequest<Cover>(RequestType.GetById, request.id);
            var entity = await _service.GetByIdAsync(
                request.id,
                request.QueryParams.IncludeAuthor ?? false,
                request.QueryParams.IncludeCover ?? false,
                request.QueryParams.IncludeCoverAndArtists ?? false);

            if (entity == null) LogAndThrowNotFoundException<Cover>(request.id);

            return _mapper.Map<CoverDTO_Response>(entity);
        }
    }
}
