using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Base;

using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class BookService : BaseService<Book>, IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(
            ILogger<IBookService> logger,
            IBaseRepository<Book> baseRepository,
            IBookRepository bookRepository
        ) : base(logger, baseRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public async Task<EntityListWithPaginationMetadata<Book>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool includeAuthor,
            bool includeCover,
            bool includeCoverAndArtists,
            bool onlyShowDeleted)
        {
            return await _bookRepository.ListPagedAsync(
                pageNumber,
                pageSize,
                orderBy,
                searchQuery,
                includeAuthor,
                includeCover,
                includeCoverAndArtists,
                onlyShowDeleted);
        }

        public async Task<Book?> GetByIdAsync(
            int id,
            bool includeAuthor,
            bool includeCover,
            bool includeCoverAndArtists)
        {
            return await _bookRepository.GetByIdAsync(
                id,
                includeAuthor,
                includeCover,
                includeCoverAndArtists);
        }
    }
}
