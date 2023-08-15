using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repository.Base;

namespace Enterprise.Solution.Repositories
{
    /// <summary>
    /// Notification Async Repository
    /// </summary>
    /// <typeparam name="Notification"></typeparam>
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<IReadOnlyList<Notification>> ListAllAsync(Guid assignedTo, bool onlyShowDeleted);
    }
}
