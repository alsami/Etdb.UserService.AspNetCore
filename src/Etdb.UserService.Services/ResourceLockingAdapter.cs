using System;
using System.Reflection;
using System.Threading.Tasks;
using Elders.RedLock;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Services
{
    public class ResourceLockingAdapter : IResourceLockingAdapter
    {
        private readonly IRedisLockManager redisLockManager;
        private const string LockPrefix = "_Resource_Lock_";

        public ResourceLockingAdapter(IRedisLockManager redisLockManager)
        {
            this.redisLockManager = redisLockManager;
        }

        public Task<bool> LockAsync(object key, TimeSpan lockSpan) => this.redisLockManager
            .LockAsync(GetCombinedKey(key), lockSpan);

        public Task UnlockAsync(object key) => this.redisLockManager
            .UnlockAsync(GetCombinedKey(key));

        private static string GetCombinedKey(object key)
            => $"{ResourceLockingAdapter.LockPrefix}{GetTypeName(key)}{key}";

        private static string GetTypeName(object key)
            => key.GetType().GetTypeInfo().Name;

    }
}