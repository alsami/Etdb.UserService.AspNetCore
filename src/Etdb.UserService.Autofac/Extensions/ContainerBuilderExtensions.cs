using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Etdb.UserService.Autofac.Modules;
using Etdb.UserService.AutoMapper.Profiles;
using Etdb.UserService.Cqrs.CommandHandler.Users;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Autofac.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void SetupDependencies(this ContainerBuilder containerBuilder,
            IHostEnvironment hostingEnvironment)
        {
            containerBuilder.AddAutoMapper(typeof(UsersProfile).Assembly);
            containerBuilder.AddMediatR(typeof(UserRegisterCommandHandler).Assembly);
            containerBuilder.RegisterModule(new DocumentDbContextModule(hostingEnvironment));
            containerBuilder.RegisterModule(new ResourceCachingModule(hostingEnvironment));
            containerBuilder.RegisterModule(new AzureServiceBusModule(hostingEnvironment));
            containerBuilder.RegisterModule(new ValidationModule(typeof(UserRegisterCommandHandler).Assembly));
            containerBuilder.RegisterModule<ServicesModule>();
            containerBuilder.RegisterModule<RepositoriesModule>();
            containerBuilder.RegisterModule<AuthenticationStrategyModule>();
        }
    }
}