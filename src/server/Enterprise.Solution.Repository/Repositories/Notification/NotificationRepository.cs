using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repository.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Enterprise.Solution.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(EnterpriseSolutionDbContext dbContext, ILogger<BaseRepository<Notification>> logger) : base(dbContext, logger) { }

        public async Task<IReadOnlyList<Notification>> ListAllAsync(Guid assignedTo, bool onlyShowDeleted)
        {
            var collection = _dbContext.Notifications as IQueryable<Notification>;

            collection = collection.Where(c => c.IsDeleted == onlyShowDeleted);

            collection = collection.Where(n => n.AssignedTo.Equals(assignedTo));

            return await collection.ToListAsync();
        }
    }
}
