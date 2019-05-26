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
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Xunit;

namespace Etdb.UserService.Bootstrap.Tests
{
    public class ContainerBuilderExtensionsIntegrationTests
    {
        [Fact]
        public void ContainerBuilderExtensions_SetupDependencies_Dependencies_Registered_And_Can_Be_Resolved()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.SetupDependencies();


            var container = containerBuilder.Build();

            Assert.True(container.IsRegistered<IRedisLockManager>(), $"{nameof(IRedisLockManager)} not registered");
            Assert.True(container.IsRegistered<IExternalAuthenticationStrategy>(),
                $"{nameof(IExternalAuthenticationStrategy)} not registered");
            Assert.True(container.IsRegistered<IProfileImageUrlFactory>(),
                $"{nameof(IProfileImageUrlFactory)} not registered");
            Assert.True(container.IsRegistered<IActionContextAccessor>(),
                $"{nameof(IActionContextAccessor)} not registered");
            Assert.True(container.IsRegistered<IHasher>(), $"{nameof(IHasher)} not registered");
            Assert.True(container.IsRegistered<IFileService>(), $"{nameof(IFileService)} not registered");
            Assert.True(container.IsRegistered<DocumentDbContext>(), $"{nameof(DocumentDbContext)} not registered");
            Assert.True(container.IsRegistered<IBus>(), $"{nameof(IBus)} not registered");
            Assert.True(container.IsRegistered<IHttpContextAccessor>(),
                $"{nameof(IHttpContextAccessor)} not registered");
            Assert.True(container.IsRegistered<IGoogleAuthenticationStrategy>(),
                $"{nameof(IGoogleAuthenticationStrategy)} not registered");
            Assert.True(container.IsRegistered<IFacebookAuthenticationStrategy>(),
                $"{nameof(IFacebookAuthenticationStrategy)} not registered");
            Assert.True(container.IsRegistered<IUsersService>(), $"{nameof(IUsersService)} not registered");
            Assert.True(container.IsRegistered<IResourceLockingAdapter>(),
                $"{nameof(IResourceLockingAdapter)} not registered");
            Assert.True(container.IsRegistered<Func<AuthenticationProvider, IExternalAuthenticationStrategy>>(),
                $"{nameof(Func<AuthenticationProvider, IExternalAuthenticationStrategy>)} not registered");
        }
    }
}