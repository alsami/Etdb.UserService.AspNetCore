using Autofac;
using Autofac.Extensions.FluentBuilder;
using AutoMapper.Extensions.Autofac.DependencyInjection;
using Elders.RedLock;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.ServiceBase.Cqrs.Abstractions.Validation;
using Etdb.ServiceBase.Cqrs.Bus;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Generics;
using Etdb.ServiceBase.Services;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.AutoMapper.Profiles;
using Etdb.UserService.Bootstrap.Services;
using Etdb.UserService.Cqrs.Handler;
using Etdb.UserService.Repositories;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;

namespace Etdb.UserService.Bootstrap.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void SetupDependencies(this ContainerBuilder containerBuilder, IHostingEnvironment environment,
            IConfiguration configuration)
        {
            containerBuilder
                .Register(ctx =>
                    new RedisLockManager(ctx.Resolve<IConfiguration>().GetSection(nameof(RedisCacheOptions))
                        .GetValue<string>("Configuration"))).As<IRedisLockManager>()
                .InstancePerLifetimeScope();

            new AutofacFluentBuilder(containerBuilder
                    .AddMediatR(typeof(UserRegisterCommandHandler).Assembly)
                    .AddAutoMapper(typeof(UsersProfile).Assembly))
                .RegisterInstance<Microsoft.Extensions.Hosting.IHostingEnvironment>(environment)
                .RegisterInstance<IConfiguration>(configuration)
                .RegisterTypeAsSingleton<ContextLessRouteProvider>()
                .RegisterTypeAsSingleton<UserProfileImageUrlFactory, IUserProfileImageUrlFactory>()
                .RegisterTypeAsSingleton<ActionContextAccessor, IActionContextAccessor>()
                .RegisterTypeAsSingleton<Hasher, IHasher>()
                .RegisterTypeAsSingleton<FileService, IFileService>()
                .RegisterTypeAsSingleton<CorsPolicyService, ICorsPolicyService>()
                .RegisterTypeAsScoped<Bus, IBus>()
                .RegisterTypeAsScoped<HttpContextAccessor, IHttpContextAccessor>()
                .RegisterTypeAsScoped<AuthService, IResourceOwnerPasswordValidator>()
                .RegisterTypeAsScoped<AuthService, IProfileService>()
                .RegisterTypeAsScoped<CachedGrantStoreService, IPersistedGrantStore>()
                .RegisterTypeAsScoped<UsersService, IUsersService>()
                .RegisterTypeAsScoped<UserServiceDbContext, DocumentDbContext>()
                .AddClosedTypeAsScoped(typeof(ICommandValidation<>),
                    new[] {typeof(UserRegisterCommandHandler).Assembly})
                .AddClosedTypeAsScoped(typeof(IDocumentRepository<,>), new[] {typeof(UserServiceDbContext).Assembly});
        }
    }
}