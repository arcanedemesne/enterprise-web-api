﻿using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Enterprise.Solution.Repositories
{
    public class EmailSubscriptionRepository : BaseRepository<EmailSubscription>, IEmailSubscriptionRepository
    {
        public EmailSubscriptionRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<EmailSubscription>> logger) : base(dbContext, logger) { }



        public async override Task<IReadOnlyList<EmailSubscription>> ListAllAsync(
            string? searchQuery,
            bool onlyShowDeleted)
        {
            var collection = _dbContext.EmailSubscriptions as IQueryable<EmailSubscription>;

            collection = collection.Where(c => c.IsDeleted == onlyShowDeleted);

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a =>
                a.FirstName.ToLower().Contains(searchQuery.ToLower())
                || a.LastName.ToLower().Contains(searchQuery.ToLower())
                || a.EmailAddress.ToLower().Contains(searchQuery.ToLower()));
            }

            return await collection.ToListAsync();
        }

        public async override Task<EntityListWithPaginationMetadata<EmailSubscription>> ListPagedAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery,
            bool onlyShowDeleted)
        {
            if (String.IsNullOrEmpty(orderBy)) orderBy = "firstName asc, lastName asc";

            var collection = _dbContext.EmailSubscriptions as IQueryable<EmailSubscription>;

            collection = collection.Where(c => c.IsDeleted == onlyShowDeleted);

            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                .Where(a =>
                a.FirstName.ToLower().Contains(searchQuery.ToLower())
                || a.LastName.ToLower().Contains(searchQuery.ToLower())
                || a.EmailAddress.ToLower().Contains(searchQuery.ToLower()));
            }

            var totalAuthorCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalAuthorCount, pageSize, pageNumber, orderBy);

            var collectionToReturn = await collection
                .OrderBy(orderBy)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return new EntityListWithPaginationMetadata<EmailSubscription>(collectionToReturn, paginationMetadata);
        }
    }
}
