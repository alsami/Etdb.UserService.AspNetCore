using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;

namespace PlaygroundBackend.Model.Modules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(componentContext =>
            {
                var profiles = componentContext.Resolve<IEnumerable<Profile>>();
                var config = new MapperConfiguration(mapperConfigurationExpression =>
                {
                    foreach (var profile in profiles)
                    {
                        mapperConfigurationExpression.AddProfile(profile);
                    }
                });

                return config;
            }).SingleInstance()
            .AutoActivate()
            .AsSelf();

            builder.Register(componentContext =>
            {
                var config = componentContext.Resolve<IComponentContext>().Resolve<MapperConfiguration>();
                return config.CreateMapper(type => componentContext.Resolve<IComponentContext>().Resolve(type));
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
