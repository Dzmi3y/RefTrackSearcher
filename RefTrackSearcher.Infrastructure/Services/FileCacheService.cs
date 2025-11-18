using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RefTrackSearcher.Core.Config;

namespace RefTrackSearcher.Infrastructure.Services
{
    public class FileCacheService : IFileCacheService
    {
        private readonly IOptions<CacheConfig> _options;
        private TimeSpan _cacheExpiration;

        public FileCacheService(IOptions<CacheConfig> options)
        {
            _options = options;
            _cacheExpiration = TimeSpan.FromMilliseconds(_options.Value.Expiration);
            Directory.CreateDirectory(_options.Value.Directory);
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration = default)
        {
            if (expiration == default)
            {
                expiration = _cacheExpiration;
            }

            var cacheFile = Path.Combine(_options.Value.Directory, $"{key}.json");

            if (File.Exists(cacheFile) && !IsExpired(cacheFile, expiration))
            {
                Console.WriteLine($"Cache hit for key: {key}");
                var cachedJson = await File.ReadAllTextAsync(cacheFile);
                return JsonSerializer.Deserialize<T>(cachedJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var result = await factory();
            var json = JsonSerializer.Serialize(result);
            await File.WriteAllTextAsync(cacheFile, json);
            Console.WriteLine($"Response cached with key: {key} (expires in {expiration.TotalMinutes} minutes)");

            return result;
        }

        public string GenerateKey(params object[] parameters)
        {
            var keyData = string.Join("_", parameters.Select(p => p?.ToString() ?? "null"));

            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyData));
            return Convert.ToBase64String(hashBytes)
                .Replace("/", "_")
                .Replace("+", "-")
                .Replace("=", "");
        }

        private bool IsExpired(string cacheFile, TimeSpan expiration)
        {
            var fileInfo = new FileInfo(cacheFile);
            var cacheAge = DateTime.Now - fileInfo.LastWriteTime;
            var isExpired = cacheAge > expiration;

            if (isExpired)
            {
                File.Delete(cacheFile);
                Console.WriteLine($"Cache expired and deleted: {Path.GetFileName(cacheFile)}");
            }

            return isExpired;
        }
    }
}