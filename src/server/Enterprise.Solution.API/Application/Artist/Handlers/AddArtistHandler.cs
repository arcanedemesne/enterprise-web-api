using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Add Command
    /// </summary>
    public class AddArtistHandler : BaseHandler<AddArtistHandler>, IRequestHandler<AddArtistCommand, ArtistDTO>
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
        public AddArtistHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<AddArtistHandler> logger,
            IMapper mapper,
            IArtistService service) : base(solutionSettings, mediator, logger, mapper)
        {
            LogInsideConstructor();
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task<ArtistDTO> Handle(AddArtistCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<AddArtistCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Artist>(RequestType.Add);

            var entity = _mapper.Map<Artist>(request.ArtistDTO);

            LogTryServiceRequest<Artist>(RequestType.Add, request.ArtistDTO.Id);
            var createdEntity = await _service.AddAsync(entity);

            if (createdEntity != null)
            {
                LogServiceRequestSuccess<Artist>(RequestType.Add, createdEntity.Id);
                return _mapper.Map<ArtistDTO>(createdEntity);
            }

            LogAndThrowNotAddedException<Artist>();
            return new ArtistDTO();
        }
    }
}
