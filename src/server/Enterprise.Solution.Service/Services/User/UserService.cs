using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Base;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            ILogger<IUserService> logger,
            IBaseRepository<User> baseRepository,
            IUserRepository userRepository
        ) : base(logger, baseRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<EntityListWithPaginationMetadata<User>> ListAllAsync(
            int pageNumber,
            int pageSize,
            string? orderBy,
            string? searchQuery = null,
            bool onlyShowDeleted = false)
        {
            return await _userRepository.ListAllAsync(
                pageNumber,
                pageSize,
                orderBy,
                searchQuery,
                onlyShowDeleted);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _userRepository.ExistsAsync(id);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
    }
}
