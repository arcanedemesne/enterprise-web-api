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
    public class AddBookHandler : BaseHandler<AddBookHandler>, IRequestHandler<AddBookCommand, BookDTO_Request>
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
        public AddBookHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<AddBookHandler> logger,
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
        public async Task<BookDTO_Request> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<AddBookCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Book>(RequestType.Add);

            var entity = _mapper.Map<Book>(request.BookDTO);

            LogTryServiceRequest<Book>(RequestType.Add);
            var createdEntity = await _service.AddAsync(entity);

            if (createdEntity != null)
            {
                LogServiceRequestSuccess<Book>(RequestType.Add, createdEntity.Id);
                return _mapper.Map<BookDTO_Request>(createdEntity);
            }

            LogAndThrowNotAddedException<Book>();
            return new BookDTO_Request();
        }
    }
}
