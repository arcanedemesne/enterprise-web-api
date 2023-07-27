using MediatR;

using Enterprise.Solution.API.Models;
using Enterprise.Solution.Service.QueryParams;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to get by Id
    /// </summary>
    public record GetAuthorByIdQuery(int Id, AuthorQueryParams QueryParams) : IRequest<AuthorDTO_Response>;
}
