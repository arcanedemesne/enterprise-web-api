using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<ICacheService> _logger;
        private readonly IDistributedCache _redisCache;

        public CacheService(ILogger<ICacheService> logger, IDistributedCache multiplexer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisCache = multiplexer ?? throw new ArgumentNullException(nameof(multiplexer));
        }

        public async Task<string?> GetAsync(string key)
        {
            var value = await _redisCache.GetStringAsync(key);

            _logger.LogInformation($"{nameof(ICacheService)}: Getting value {value} from cache key '{key}'");

            return value;
        }

        public async Task PutAsync(string key, string serializedValue)
        {
            _logger.LogInformation($"Putting value {serializedValue} for cache key '{key}'");

            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(1);
            options.SlidingExpiration = TimeSpan.FromMinutes(20);

            await _redisCache.SetStringAsync(key, serializedValue, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _redisCache.RemoveAsync(key);
        }

    }
}
