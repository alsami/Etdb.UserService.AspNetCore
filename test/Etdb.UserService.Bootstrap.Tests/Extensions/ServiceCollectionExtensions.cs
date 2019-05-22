using Etdb.ServiceBase.Constants;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Bootstrap.Tests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDistributedCaching(this IServiceCollection services,
            IConfiguration configuration)
        {
            var optionsFromConfig = configuration.GetSection(nameof(RedisCacheOptions))
                .Get<RedisCacheOptions>();


            return services.AddStackExchangeRedisCache(redisCacheOptions =>
            {
                redisCacheOptions.Configuration = optionsFromConfig.Configuration;
                redisCacheOptions.InstanceName = optionsFromConfig.InstanceName;
            });
        }
    }
}