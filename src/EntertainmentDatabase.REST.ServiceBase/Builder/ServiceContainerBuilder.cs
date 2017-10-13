using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using EntertainmentDatabase.REST.ServiceBase.Configuration;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.ServiceBase.Generics.Facades;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Net;

namespace EntertainmentDatabase.REST.ServiceBase.Builder
{
    public class ServiceContainerBuilder
    {
        private readonly IServiceCollection serviceCollection;
        private readonly ContainerBuilder containerBuilder;
        private readonly string projectName;
        public ServiceContainerBuilder(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder;
        }


        public ServiceContainerBuilder(IServiceCollection serviceCollection, string projectName)
        {
            this.serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            this.projectName = projectName ?? throw new ArgumentNullException(nameof(projectName));
            this.containerBuilder = new ContainerBuilder();
        }

        public ServiceContainerBuilder UseConfiguration(IConfigurationRoot configurationRoot)
        {
            this.containerBuilder
                .RegisterInstance(configurationRoot)
                .SingleInstance();
            return this;
        }

        public ServiceContainerBuilder UseEnvironment(IHostingEnvironment hostingEnvironment)
        {
            this.containerBuilder
                .RegisterInstance(hostingEnvironment)
                .SingleInstance();
            return this;
        }

        public ServiceContainerBuilder UseGenericRepositoryPattern<T>() where T : DbContext
        {    
            this.containerBuilder.RegisterGeneric(typeof(EntityRepository<>))
                .As(typeof(IEntityRepository<>))
                .InstancePerRequest()
                .WithParameter(new ResolvedParameter(
                    (parameterInfo, componentContext) => parameterInfo.ParameterType == typeof(DbContext),
                    (parameterInfo, componentContext) => componentContext.Resolve<T>()))
                .InstancePerLifetimeScope();

            return this;
        }

        public ServiceContainerBuilder RegisterTypeAsSingleton<TImplementation>()
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .SingleInstance();
            return this;
        }

        public ServiceContainerBuilder RegisterTypeAsSingleton<TImplementation, TInterface>()
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>()
                .AsSelf()
                .SingleInstance();

            return this;
        }
    }
}
