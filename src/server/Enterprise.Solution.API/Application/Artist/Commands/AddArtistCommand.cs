using Enterprise.Solution.API.Models;

using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Add
    /// </summary>
    public record AddArtistCommand(ModelStateDictionary ModelState, ArtistDTO_Request ArtistDTO) : IRequest<ArtistDTO_Request>;
}
