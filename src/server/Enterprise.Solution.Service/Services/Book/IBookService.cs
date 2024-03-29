﻿using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public interface IBookService : IBaseService<Book>
    {
        public Task<EntityListWithPaginationMetadata<Book>> ListPagedAsync(
            int pageNumber,
            int PageSize,
            string? orderBy,
            string? searchQuery,
            bool includeAuthor,
            bool includeCover,
            bool includeCoverAndArtists,
            bool onlyShowDeleted);

        public Task<Book?> GetByIdAsync(
            int id,
            bool includeAuthor,
            bool includeCover,
            bool includeCoverAndArtists);
    }
}
