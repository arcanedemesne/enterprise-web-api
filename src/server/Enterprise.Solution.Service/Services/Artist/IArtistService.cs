﻿using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public interface IArtistService : IBaseService<Artist>
    {
        public Task<EntityListWithPaginationMetadata<Artist>> ListPagedAsync(
            int pageNumber,
            int PageSize,
            string? orderBy,
            string? searchQuery,
            bool includeCovers,
            bool includeCoversWithBook,
            bool includeCoversWithBookAndAuthor,
            bool onlyShowDeleted);

        public Task<Artist?> GetByIdAsync(
            int id,
            bool includeCovers,
            bool includeCoversWithBook,
            bool includeCoversWithBookAndAuthor);
    }
}
