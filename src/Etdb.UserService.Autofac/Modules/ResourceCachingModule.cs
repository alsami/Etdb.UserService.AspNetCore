using System;
using Autofac;
using Elders.RedLock;
using Etdb.UserService.Autofac.Extensions;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Autofac.Modules
{
    public class ResourceCachingModule : Module
    {
        private readonly IHostEnvironment environment;

        public ResourceCachingModule(IHostEnvironment environment) =>
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));

        protected override void Load(ContainerBuilder builder)
        {
            if (this.environment.IsAzureDevelopment())
            {
                builder.RegisterType<MemoryResourceLockingAdapter>()
                    .As<IResourceLockingAdapter>()
                    .SingleInstance();

                builder.Register(_ => new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions
                    {
                        ExpirationScanFrequency = TimeSpan.FromMinutes(10)
                    })))
                    .As<IDistributedCache>()
                    .SingleInstance();

                return;
            }

            builder.RegisterType<ResourceLockingAdapter>()
                .As<IResourceLockingAdapter>()
                .InstancePerLifetimeScope();

            builder.Register(RedisLockManagerResolver)
                .As<IRedisLockManager>()
                .InstancePerLifetimeScope();
        }

        private static IRedisLockManager RedisLockManagerResolver(IComponentContext componentContext) =>
            new RedisLockManager(new RedLockOptions
            {
                LockRetryCount = 2
            }, componentContext.Resolve<IOptions<RedisCacheOptions>>().Value.Configuration);
    }
}