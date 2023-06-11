using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;

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
            _logger.LogInformation($"{nameof(AddArtistHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task<ArtistDTO> Handle(AddArtistCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(AddArtistHandler)} handler invoked with request type {nameof(AddArtistCommand)}.");

            if (!request.ModelState.IsValid)
            {
                var ex = new InvalidModelException(RequestType.Add);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var emailSubscription = _mapper.Map<Artist>(request.ArtistDTO);

            _logger.LogInformation($"Try to add a {nameof(Artist)}.");
            var createdArtist = await _service.AddAsync(emailSubscription);

            if (createdArtist != null)
            {
                _logger.LogInformation($"{nameof(Artist)} with id {createdArtist.Id} was successfully added.");
                return _mapper.Map<ArtistDTO>(createdArtist);
            }
            else
            {
                var ex = new NotCreatedException(typeof(Artist));
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
