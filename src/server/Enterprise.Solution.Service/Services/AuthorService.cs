using AutoMapper;
using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Base;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class AuthorService : BaseService<Author>, IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(
            ILogger<IAuthorService> logger,
            IBaseRepository<Author> baseRepository,
            IAuthorRepository authorRepository
        ) : base(logger, baseRepository)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        }

        public async Task<(IReadOnlyList<Author>, PaginationMetadata)> ListAllAsync(
            string? filter,
            string? search,
            int pageNumber,
            int pageSize
        )
        {
            if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }

            return await _authorRepository.ListAllAsync(filter, search, pageNumber, pageSize);
        }
    }
}
