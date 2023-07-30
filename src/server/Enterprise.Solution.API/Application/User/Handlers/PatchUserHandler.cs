﻿using AutoMapper;
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
    public class PatchUserHandler : BaseHandler<PatchUserHandler>, IRequestHandler<PatchUserCommand>
    {
        private readonly IUserService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public PatchUserHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<PatchUserHandler> logger,
            IMapper mapper,
            IUserService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(PatchUserCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<PatchUserCommand>();

            LogCheckIfExists<User>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<User>(request.Id);

            LogTryServiceRequest<User>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);
            if (entity == null) LogAndThrowNotFoundException<User>(request.Id);

            var patchedEntity = _mapper.Map<UserDTO_Request>(entity);

            LogTryServiceRequest<User>(RequestType.Patch, request.Id);
            request.JsonPatchDocument.ApplyTo(patchedEntity, request.ModelState);

            if (!request.ModelState.IsValid || !request.TryValidateModel(patchedEntity))
                LogAndThrowInvalidModelException<User>(RequestType.Patch);

            _mapper.Map(patchedEntity, entity);

            try
            {
                LogTryServiceRequest<User>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
            }
            catch (Exception)
            {
                LogAndThrowNotPatchedException<User>(request.Id);
            }
            return;
        }
    }
}
