using System;
using Autofac;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Authentication.Strategies;
using Etdb.UserService.Domain.Enums;

namespace Etdb.UserService.Autofac.Modules
{
    public class AuthenticationStrategyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GoogleAuthenticationStrategy>()
                .As<IGoogleAuthenticationStrategy>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FacebookAuthenticationStrategy>()
                .As<IFacebookAuthenticationStrategy>()
                .InstancePerLifetimeScope();

            builder.Register<Func<AuthenticationProvider, IExternalAuthenticationStrategy>>(outerComponentContext =>
            {
                var innerComponent = outerComponentContext.Resolve<IComponentContext>();

                return authenticationProvier =>
                {
                    switch (authenticationProvier)
                    {
                        case AuthenticationProvider.Google:
                        {
                            return innerComponent.Resolve<IGoogleAuthenticationStrategy>();
                        }

                        case AuthenticationProvider.Facebook:
                        {
                            return innerComponent.Resolve<IFacebookAuthenticationStrategy>();
                        }

                        case AuthenticationProvider.Twitter:
                        {
                            throw new NotImplementedException();
                        }

                        case AuthenticationProvider.UsernamePassword:
                            throw new ArgumentOutOfRangeException(nameof(authenticationProvier));
                        default:
                            throw new ArgumentOutOfRangeException(nameof(authenticationProvier));
                    }
                };
            });
        }
    }
}