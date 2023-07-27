using MediatR;

using Enterprise.Solution.API.Models;
using Enterprise.Solution.Service.QueryParams;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to get by Id
    /// </summary>
    public record GetBookByIdQuery(int id, BookQueryParams QueryParams) : IRequest<BookDTO_Response>;
}
