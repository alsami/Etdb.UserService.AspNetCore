using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.FluentBuilder;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.ServiceBase.Services;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Authentication.Strategies;
using Etdb.UserService.Autofac.Modules;
using Etdb.UserService.AutoMapper.Profiles;
using Etdb.UserService.Cqrs.CommandHandler.Users;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Repositories;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Autofac.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void SetupDependencies(this ContainerBuilder containerBuilder,
            IHostEnvironment hostingEnvironment)
        {
            new AutofacFluentBuilder(containerBuilder
                    .AddMediatR(typeof(UserRegisterCommandHandler).Assembly)
                    .AddAutoMapper(typeof(UsersProfile).Assembly))
                .ApplyModule(new DocumentDbContextModule(hostingEnvironment))
                .ApplyModule(new ResourceCachingModule(hostingEnvironment))
                .ApplyModule(new AzureServiceBusModule(hostingEnvironment))
                .RegisterResolver(ContainerBuilderExtensions.ExternalAuthenticationStrategyResolver)
                .RegisterTypeAsSingleton<ActionContextAccessor, IActionContextAccessor>()
                .RegisterTypeAsSingleton<Hasher, IHasher>()
                .RegisterTypeAsSingleton<FileService, IFileService>()
                .RegisterTypeAsSingleton<ImageCompressionService, IImageCompressionService>()
                .RegisterTypeAsScoped<HttpContextAccessor, IHttpContextAccessor>()
                .RegisterTypeAsScoped<GoogleAuthenticationStrategy, IGoogleAuthenticationStrategy>()
                .RegisterTypeAsScoped<FacebookAuthenticationStrategy, IFacebookAuthenticationStrategy>()
                .RegisterTypeAsScoped<UsersService, IUsersService>()
                .RegisterTypeAsScoped<UsersCachingRepository, IUsersRepository>()
                .RegisterTypeAsScoped<ApplicationUser, IApplicationUser>()
                .RegisterTypeAsScoped<UserUrlFactory, IUserUrlFactory>()
                .AddClosedTypeAsScoped(typeof(AbstractValidator<>),
                    new[] {typeof(UserRegisterCommandHandler).Assembly})
                .AddClosedTypeAsScoped(typeof(GenericDocumentRepository<,>), new[] {typeof(UserServiceDbContext).Assembly});
        }


        private static IExternalAuthenticationStrategy ExternalAuthenticationStrategyResolver(
            IComponentContext componentContext,
            IEnumerable<Parameter> @params)
        {
            var provider = @params.TypedAs<AuthenticationProvider>();

            switch (provider)
            {
                case AuthenticationProvider.Google:
                {
                    return componentContext.Resolve<IGoogleAuthenticationStrategy>();
                }

                case AuthenticationProvider.Facebook:
                {
                    return componentContext.Resolve<IFacebookAuthenticationStrategy>();
                }

                case AuthenticationProvider.Twitter:
                {
                    throw new NotImplementedException();
                }

                case AuthenticationProvider.UsernamePassword:
                    throw new ArgumentOutOfRangeException(nameof(provider));
                default:
                    throw new ArgumentOutOfRangeException(nameof(provider));
            }
        }
    }
}