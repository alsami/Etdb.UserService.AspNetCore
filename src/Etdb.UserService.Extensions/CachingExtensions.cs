using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Etdb.UserService.Extensions
{
    public static class CachingExtensions
    {
        public static async Task AddOrUpdateAsync<T, TKey>(this IDistributedCache cache, TKey key, T @object,
            DistributedCacheEntryOptions options = null, CancellationToken token = default(CancellationToken))
            where T : class where TKey : IEquatable<TKey>
        {
            var usedOptions = options ?? new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            var entry = await cache.GetAsync(key.ToString(), token);

            if (entry != null)
            {
                await cache.RemoveAsync(key.ToString(), token);
            }
            
            await cache.SetStringAsync(key.ToString(), JsonConvert.SerializeObject(@object), usedOptions, token);
        }

        public static async Task<T> GetAsync<T, TKey>(this IDistributedCache cache, TKey key,
            CancellationToken token = default(CancellationToken)) where T : class where TKey : IEquatable<TKey>
        {
            var @string = await cache.GetStringAsync(key.ToString(), token);

            if (@string == null || string.IsNullOrWhiteSpace(@string))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(@string);
        }
    }
}