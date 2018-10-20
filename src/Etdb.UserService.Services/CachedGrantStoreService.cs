using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etdb.UserService.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Distributed;

namespace Etdb.UserService.Services
{
    public class CachedGrantStoreService : IPersistedGrantStore
    {
        private const string TokenKeyPrefix = "etdb_token_";
        private readonly IDistributedCache cache;

        public CachedGrantStoreService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            var cachingOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = grant.Expiration.GetValueOrDefault()
            };

            await this.cache.AddOrUpdateAsync(grant.Key, grant, cachingOptions);

            await this.cache.AddOrUpdateAsync($"{CachedGrantStoreService.TokenKeyPrefix}{grant.SubjectId}", grant,
                cachingOptions);
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            return await this.cache.FindAsync<PersistedGrant, string>(key);
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var grants =
                await this.cache.FindAsync<IEnumerable<PersistedGrant>, string>(
                    $"{CachedGrantStoreService.TokenKeyPrefix}{subjectId}");

            return grants.ToArray().AsEnumerable();
        }

        public async Task RemoveAsync(string key)
        {
            await this.cache.RemoveAsync(key);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var grants =
                await this.cache.FindAsync<IEnumerable<PersistedGrant>, string>(
                    $"{CachedGrantStoreService.TokenKeyPrefix}{subjectId}");

            foreach (var grant in grants.Where(grant => grant.ClientId == clientId).ToArray())
                await this.cache.RemoveAsync(grant.Key);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var grants =
                await this.cache.FindAsync<IEnumerable<PersistedGrant>, string>(
                    $"{CachedGrantStoreService.TokenKeyPrefix}{subjectId}");

            foreach (var grant in grants.Where(grant => grant.ClientId == clientId && grant.Type == type).ToArray())
                await this.cache.RemoveAsync(grant.Key);
        }
    }
}