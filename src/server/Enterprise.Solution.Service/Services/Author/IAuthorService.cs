using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public interface IAuthorService : IBaseService<Author>
    {
        public Task<EntityListWithPaginationMetadata<Author>> ListAllAsync(
            int pageNumber,
            int PageSize,
            string? orderBy,
            string? searchQuery,
            bool includeBooks,
            bool includeBooksWithCover,
            bool includeBooksWithCoverAndAuthors,
            bool onlyShowDeleted);

        public Task<Author?> GetByIdAsync(
            int id,
            bool includeBooks,
            bool includeBooksWithCover,
            bool includeBooksWithCoverAndAuthors);
    }
}
