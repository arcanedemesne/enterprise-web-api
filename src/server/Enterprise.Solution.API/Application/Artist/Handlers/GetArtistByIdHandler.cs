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
    public class GetArtistByIdHandler : BaseHandler<GetArtistByIdHandler>, IRequestHandler<GetArtistByIdQuery, ArtistDTO_Response>
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
        public GetArtistByIdHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<GetArtistByIdHandler> logger,
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
        public async Task<ArtistDTO_Response> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<GetArtistByIdQuery>();

            LogTryServiceRequest<Artist>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null) LogAndThrowNotFoundException<Artist>(request.Id);

            return _mapper.Map<ArtistDTO_Response>(entity);
        }
    }
}
