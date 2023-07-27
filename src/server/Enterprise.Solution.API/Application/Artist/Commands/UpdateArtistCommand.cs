using Enterprise.Solution.API.Models;

using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Update
    /// </summary>
    public record UpdateArtistCommand(int Id, ModelStateDictionary ModelState, ArtistDTO_Request ArtistDTO) : IRequest;
}
