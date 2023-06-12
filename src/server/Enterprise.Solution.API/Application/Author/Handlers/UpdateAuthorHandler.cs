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
    public class UpdateAuthorHandler : BaseHandler<UpdateAuthorHandler>, IRequestHandler<UpdateAuthorCommand>
    {
        private readonly IAuthorService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public UpdateAuthorHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<UpdateAuthorHandler> logger,
            IMapper mapper,
            IAuthorService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<UpdateAuthorCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Author>(RequestType.Update);

            LogCheckIfExists<Author>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Author>(request.Id);

            if (!request.Id.Equals(request.AuthorDTO.Id))
                LogAndThrowNotUpdatedException<Author>(request.Id);

            LogTryServiceRequest<Author>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null) LogAndThrowNotUpdatedException<Author>(request.Id);
            _mapper.Map(request.AuthorDTO, entity);

            try
            {
                LogTryServiceRequest<Author>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
                return;
            }
            catch (Exception)
            {
                LogAndThrowNotUpdatedException<Author>(request.Id);
            }
        }
    }
}
