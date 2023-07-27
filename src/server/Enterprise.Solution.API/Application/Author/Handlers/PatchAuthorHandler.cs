using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Patch Command
    /// </summary>
    public class PatchAuthorHandler : BaseHandler<PatchAuthorHandler>, IRequestHandler<PatchAuthorCommand>
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
        public PatchAuthorHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<PatchAuthorHandler> logger,
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
        public async Task Handle(PatchAuthorCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<PatchAuthorCommand>();

            LogCheckIfExists<Author>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Author>(request.Id);

            LogTryServiceRequest<Author>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);
            if (entity == null) LogAndThrowNotFoundException<Author>(request.Id);

            var patchedEntity = _mapper.Map<AuthorDTO_Request>(entity);

            LogTryServiceRequest<Author>(RequestType.Patch, request.Id);
            request.JsonPatchDocument.ApplyTo(patchedEntity, request.ModelState);

            if (!request.ModelState.IsValid || !request.TryValidateModel(patchedEntity))
                LogAndThrowInvalidModelException<Author>(RequestType.Patch);

            _mapper.Map(patchedEntity, entity);

            try
            {
                LogTryServiceRequest<Author>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
            }
            catch (Exception)
            {
                LogAndThrowNotPatchedException<Author>(request.Id);
            }
            return;
        }
    }
}
