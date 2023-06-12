using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Update
    /// </summary>
    public record UpdateEmailSubscriptionCommand(int Id, ModelStateDictionary ModelState, EmailSubscriptionDTO EmailSubscriptionDTO) : IRequest;
}
