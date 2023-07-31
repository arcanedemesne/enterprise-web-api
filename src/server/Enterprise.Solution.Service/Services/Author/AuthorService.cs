using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
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

        public async Task<EntityListWithPaginationMetadata<Author>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeBooks,
            bool includeBooksWithCover,
            bool includeBooksWithCoverAndArtists,
            bool onlyShowDeleted)
        {
            return await _authorRepository.ListPagedAsync(
                pageNumber,
                pageSize,
                orderBy,
                searchQuery,
                includeBooks,
                includeBooksWithCover,
                includeBooksWithCoverAndArtists,
                onlyShowDeleted);
        }

        public async Task<Author?> GetByIdAsync(
            int id,
            bool includeBooks,
            bool includeBooksWithCover,
            bool includeBooksWithCoverAndArtists)
        {
            return await _authorRepository.GetByIdAsync(
                id,
                includeBooks,
                includeBooksWithCover,
                includeBooksWithCoverAndArtists);
        }
    }
}
