using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Etdb.UserService.Services
{
    public class MemoryResourceLockingAdapter : IResourceLockingAdapter
    {
        private readonly ILogger<MemoryResourceLockingAdapter> logger;

        private static readonly ConcurrentDictionary<object, DateTime> LockedKeysByDateTime 
            = new ConcurrentDictionary<object, DateTime>();

        public MemoryResourceLockingAdapter(ILogger<MemoryResourceLockingAdapter> logger)
        {
            this.logger = logger;
            this.RunCleanupThread();
        }

        public Task<bool> LockAsync(object key, TimeSpan lockSpan)
            => Task.FromResult(MemoryResourceLockingAdapter.LockedKeysByDateTime.TryAdd(key, DateTime.UtcNow.Add(lockSpan)));

        public Task UnlockAsync(object key) => Task.FromResult(MemoryResourceLockingAdapter.LockedKeysByDateTime.TryRemove(key, out _));

        private void RunCleanupThread()
        {
            var thread = new Thread(() =>
            {
                foreach (var key in MemoryResourceLockingAdapter.LockedKeysByDateTime.Keys)
                {
                    this.logger.LogInformation("Checking if key {key} needs to be removed", key);
                    if (MemoryResourceLockingAdapter.LockedKeysByDateTime[key] >= DateTime.UtcNow) return;

                    this.logger.LogInformation("Removing value for key {key}!", key);
                    MemoryResourceLockingAdapter.LockedKeysByDateTime.TryRemove(key, out _);
                }
                
                Thread.Sleep(TimeSpan.FromMinutes(1));
            });
            
            thread.Start();
        }
    }
}