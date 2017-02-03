using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Module = Autofac.Module;

namespace UltimateCoreWebAPI.Model.Modules
{
    public class AutoMapperModule : Module
    {
        private const string FirstPartOfProjectAssemblyName = "UltimateCoreWebAPI";

        protected override void Load(ContainerBuilder builder)
        {
            // load all referenced libraries
            // that are part of this project
            var assemblies = new List<Assembly>();
            Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .Where(assemblyName => assemblyName.FullName.StartsWith(AutoMapperModule.FirstPartOfProjectAssemblyName))
                .ToList()
                .ForEach(assemblyName => assemblies.Add(Assembly.Load(assemblyName)));

            // register the assemblies of type Profile
            // this will register all available mapping profiles
            assemblies.ForEach(assembly =>
            {
                builder.RegisterAssemblyTypes(assembly)
                    .Where(type => type.IsAssignableTo<Profile>())
                    .As<Profile>()
                    .SingleInstance();
            });

            // register all AutoMapper profiles
            // create the mapperconfiguration and register it as single instance
            builder.Register(componentContext =>
            {
                var config = new MapperConfiguration(mapperConfigurationExpression =>
                {
                    foreach (var profile in componentContext.Resolve<IEnumerable<Profile>>())
                    {
                        mapperConfigurationExpression.AddProfile(profile);
                    }
                });

                return config;
            }).SingleInstance()
            .AutoActivate()
            .AsSelf();

            // resolve the mapperconfiguration and create the mapper
            // register it as singleinstance
            builder.Register(componentContext =>
            {
                return componentContext
                    .Resolve<IComponentContext>()
                    .Resolve<MapperConfiguration>()
                    .CreateMapper(type => componentContext.Resolve<IComponentContext>()
                    .Resolve(type));
            })
            .As<IMapper>()
            .SingleInstance();

            base.Load(builder);
        }
    }
}
