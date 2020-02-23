using Autofac;
using Etdb.UserService.Repositories;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Autofac.Modules
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UsersCachingRepository>()
                .As<IUsersRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SecurityRolesRepository>()
                .As<ISecurityRolesRepository>()
                .InstancePerLifetimeScope();
        }
    }
}