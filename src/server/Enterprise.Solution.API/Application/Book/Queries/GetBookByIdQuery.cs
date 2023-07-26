using MediatR;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to get by Id
    /// </summary>
    public record GetBookByIdQuery(int Id) : IRequest<BookDTO_Response>;
}
