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
    public class UpdateBookHandler : BaseHandler<UpdateBookHandler>, IRequestHandler<UpdateBookCommand>
    {
        private readonly IBookService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public UpdateBookHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<UpdateBookHandler> logger,
            IMapper mapper,
            IBookService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<UpdateBookCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Book>(RequestType.Update);

            LogCheckIfExists<Book>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Book>(request.Id);

            if (!request.Id.Equals(request.BookDTO.Id))
                LogAndThrowNotUpdatedException<Book>(request.Id);

            LogTryServiceRequest<Book>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null) LogAndThrowNotUpdatedException<Book>(request.Id);
            _mapper.Map(request.BookDTO, entity);

            try
            {
                LogTryServiceRequest<Book>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
                return;
            }
            catch (Exception)
            {
                LogAndThrowNotUpdatedException<Book>(request.Id);
            }
        }
    }
}
