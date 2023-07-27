using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Patch
    /// </summary>
    public record PatchEmailSubscriptionCommand(int Id, ModelStateDictionary ModelState, Func<object, bool> TryValidateModel, JsonPatchDocument<EmailSubscriptionDTO_Request> JsonPatchDocument) : IRequest;
}
