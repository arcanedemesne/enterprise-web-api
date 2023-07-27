using MediatR;

using Enterprise.Solution.API.Models;
using Enterprise.Solution.Service.QueryParams;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to get by Id
    /// </summary>
    public record GetArtistByIdQuery(int Id, ArtistQueryParams QueryParams) : IRequest<ArtistDTO_Response>;
}
