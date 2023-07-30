using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Repository.Base;

namespace Enterprise.Solution.Repositories
{
    /// <summary>
    /// User Async Repository
    /// </summary>
    /// <typeparam name="User"></typeparam>
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<bool> ExistsAsync(Guid id);
        public Task<User?> GetByIdAsync(Guid id);
    }
}
