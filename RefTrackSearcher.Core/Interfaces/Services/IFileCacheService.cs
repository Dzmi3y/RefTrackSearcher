public interface IFileCacheService
{
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration = default);
    string GenerateKey(params object[] parameters);
}