using MediatR;

using Enterprise.Solution.API.Models;
using Enterprise.Solution.Service.QueryParams;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to get by Id
    /// </summary>
    public record GetCoverByIdQuery(int id, CoverQueryParams QueryParams) : IRequest<CoverDTO_Response>;
}
