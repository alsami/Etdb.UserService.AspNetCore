using Autofac;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.ServiceBase.Services;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Authentication.Strategies;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Etdb.UserService.Autofac.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Hasher>()
                .As<IHasher>()
                .SingleInstance();

            builder.RegisterType<FileService>()
                .As<IFileService>()
                .SingleInstance();

            builder.RegisterType<ImageCompressionService>()
                .As<IImageCompressionService>()
                .SingleInstance();

            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GoogleAuthenticationStrategy>()
                .As<IGoogleAuthenticationStrategy>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FacebookAuthenticationStrategy>()
                .As<IFacebookAuthenticationStrategy>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UsersService>()
                .As<IUsersService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUser>()
                .As<IApplicationUser>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserUrlFactory>()
                .As<IUserUrlFactory>()
                .InstancePerLifetimeScope();
        }
    }
}