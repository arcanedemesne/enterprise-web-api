using Enterprise.Solution.Service.QueryParams;
using Enterprise.Solution.Data.Models;
using MediatR;

namespace Enterprise.Solution.Application.Queries
{
    /// <summary>
    /// Query to return an EmailSubscription by its id
    /// </summary>
    public record GetEmailSubscriptionByIdQuery(int id) : IRequest<EmailSubscription>;
}
