using Enterprise.Solution.API.Models;

using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Add
    /// </summary>
    public record AddEmailSubscriptionCommand(ModelStateDictionary ModelState, EmailSubscriptionDTO EmailSubscriptionDTO) : IRequest<EmailSubscriptionDTO>;
}
