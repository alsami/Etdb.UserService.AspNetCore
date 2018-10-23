using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elders.RedLock;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Services
{
    public class ResourceLockingAdapter : IResourceLockingAdapter
    {
        private readonly IRedisLockManager redisLockManager;
        private const string LockPrefix = "resourcelock_";

        public ResourceLockingAdapter(IRedisLockManager redisLockManager)
        {
            this.redisLockManager = redisLockManager;
        }

        public async Task<bool> LockAsync(object key, TimeSpan lockSpan)
        {
            return await this.redisLockManager
                .LockAsync($"{ResourceLockingAdapter.LockPrefix}{key}", lockSpan);
        }

        public async Task UnlockAsync(object key)
        {
            await this.redisLockManager
                .UnlockAsync($"{ResourceLockingAdapter.LockPrefix}{key}");
        }
    }
}