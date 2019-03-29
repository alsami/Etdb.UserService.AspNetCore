using System;
using Autofac;
using Elders.RedLock;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Bootstrap.Extensions;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Etdb.UserService.Bootstrap.Tests
{
    public class ContainerBuilderExtensionsIntegrationTests
    {
        [Fact]
        public void ContainerBuilderExtensions_SetupDependencies_Dependencies_Registered_And_Can_Be_Resolved()
        {
            var configuration = new ConfigurationBuilder()
                .Build();

            var env = new HostingEnvironment
            {
                EnvironmentName = "Development"
            };

            var containerBuilder = new ContainerBuilder();

            containerBuilder.SetupDependencies(env, configuration);
            

            var container = containerBuilder.Build();
            
            Assert.True(container.IsRegistered<IRedisLockManager>());
            Assert.True(container.IsRegistered<IExternalAuthenticationStrategy>());
            Assert.True(container.IsRegistered<IHostingEnvironment>());
            Assert.True(container.IsRegistered<IConfiguration>());
            Assert.True(container.IsRegistered<ContextLessRouteProvider>());
            Assert.True(container.IsRegistered<IUserProfileImageUrlFactory>());
            Assert.True(container.IsRegistered<IActionContextAccessor>());
            Assert.True(container.IsRegistered<IHasher>());
            Assert.True(container.IsRegistered<IFileService>());
            Assert.True(container.IsRegistered<DocumentDbContext>());
            Assert.True(container.IsRegistered<IBus>());
            Assert.True(container.IsRegistered<IHttpContextAccessor>());
            Assert.True(container.IsRegistered<IProfileService>());
            Assert.True(container.IsRegistered<IResourceOwnerPasswordValidator>());
            Assert.True(container.IsRegistered<IGoogleAuthenticationStrategy>());
            Assert.True(container.IsRegistered<IFacebookAuthenticationStrategy>());
            Assert.True(container.IsRegistered<IExtensionGrantValidator>());
            Assert.True(container.IsRegistered<IPersistedGrantStore>());
            Assert.True(container.IsRegistered<IUsersService>());
            Assert.True(container.IsRegistered<IResourceLockingAdapter>());
            Assert.True(container.IsRegistered<Func<AuthenticationProvider, IExternalAuthenticationStrategy>>());
        }
    }
}