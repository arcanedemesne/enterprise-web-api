using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;

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
            _logger.LogInformation($"{nameof(UpdateAuthorHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(UpdateAuthorHandler)} handler invoked with request type {nameof(UpdateAuthorCommand)}.");

            if (!request.ModelState.IsValid)
            {
                var ex = new InvalidModelException(RequestType.Update);
                _logger.LogError(ex.Message, ex.InnerException);
                throw ex;
            }

            _logger.LogInformation($"Check if {nameof(Author)} with id {request.Id} exists.");
            var exists = await _service.ExistsAsync(request.Id);

            if (!exists)
            {
                var ex = new NotFoundException(nameof(Author), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            if (!request.Id.Equals(request.AuthorDTO.Id))
            {
                var message = $"Incorrect id provided for {nameof(Author)} with id {request.Id}.";
                _logger.LogError(message);
                throw new NotUpdatedException(message);
            }

            _logger.LogInformation($"Try to get {nameof(Author)} entity with id {request.Id}.");
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity != null)
            {
                _mapper.Map(request.AuthorDTO, entity);

                try
                {
                    _logger.LogInformation($"Try to update a {nameof(Author)} with id {request.Id}.");
                    await _service.UpdateAsync(entity);
                }
                catch (Exception ex)
                {
                    var nEx = new NotUpdatedException(typeof(Author), request.Id);
                    var fEx = new NotUpdatedException(nEx.Message, ex);
                    _logger.LogError(fEx.Message, fEx.InnerException);
                    throw fEx;
                }
                return;
            }
            else
            {
                var ex = new NotUpdatedException(typeof(Author), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
