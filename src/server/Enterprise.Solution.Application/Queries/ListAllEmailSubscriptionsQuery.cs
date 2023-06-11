using Enterprise.Solution.Service.QueryParams;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using MediatR;

namespace Enterprise.Solution.Application.Queries
{
    /// <summary>
    /// Query to return List of EmailSubscriptions
    /// </summary>
    public record ListAllEmailSubscriptionsQuery(EmailSubscriptionPagedQueryParams queryParams) : IRequest<EntityListWithPaginationMetadata<EmailSubscription>>;
}
