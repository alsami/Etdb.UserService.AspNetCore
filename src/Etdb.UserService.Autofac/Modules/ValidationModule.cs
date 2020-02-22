using System;
using System.Reflection;
using Autofac;
using FluentValidation;
using Module = Autofac.Module;

namespace Etdb.UserService.Autofac.Modules
{
    public class ValidationModule : Module
    {
        private readonly Assembly assembly;

        public ValidationModule(Assembly assembly)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.assembly)
                .AsClosedTypesOf(typeof(AbstractValidator<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}