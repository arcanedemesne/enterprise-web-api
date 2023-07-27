using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Update Command
    /// </summary>
    public class UpdateCoverHandler : BaseHandler<UpdateCoverHandler>, IRequestHandler<UpdateCoverCommand>
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
        public UpdateCoverHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<UpdateCoverHandler> logger,
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
        public async Task Handle(UpdateCoverCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<UpdateCoverCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Cover>(RequestType.Update);

            LogCheckIfExists<Cover>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Cover>(request.Id);

            if (!request.Id.Equals(request.CoverDTO.Id))
                LogAndThrowNotUpdatedException<Cover>(request.Id);

            LogTryServiceRequest<Cover>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null) LogAndThrowNotUpdatedException<Cover>(request.Id);
            _mapper.Map(request.CoverDTO, entity);

            try
            {
                LogTryServiceRequest<Cover>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
                return;
            }
            catch (Exception)
            {
                LogAndThrowNotUpdatedException<Cover>(request.Id);
            }
        }
    }
}
