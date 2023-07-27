using MediatR;

using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.QueryParams;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to List All
    /// </summary>
    public record ListAllCoversQuery(CoverPagedQueryParams QueryParams) : IRequest<EntityListWithPaginationMetadata<Cover>>;
}
