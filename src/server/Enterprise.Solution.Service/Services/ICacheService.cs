namespace Enterprise.Solution.Service.Services
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string key);

        Task PutAsync(string key, string serializedValue);

        Task RemoveAsync(string key);
    }
}
