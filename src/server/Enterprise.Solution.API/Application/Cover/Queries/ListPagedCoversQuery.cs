using MediatR;

using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.QueryParams;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to List Paged
    /// </summary>
    public record ListPagedCoversQuery(CoverPagedQueryParams QueryParams) : IRequest<EntityListWithPaginationMetadata<Cover>>;
}
