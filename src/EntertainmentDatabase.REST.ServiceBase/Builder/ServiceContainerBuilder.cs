using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using EntertainmentDatabase.REST.ServiceBase.Configuration;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using EntertainmentDatabase.REST.ServiceBase.Generics.Facades;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EntertainmentDatabase.REST.ServiceBase.Builder
{
    public class ServiceContainerBuilder
    {
        private readonly IServiceCollection serviceCollection;
        private readonly ContainerBuilder containerBuilder;
        private readonly string projectName;

        public ServiceContainerBuilder(IServiceCollection serviceCollection, string projectName)
        {
            this.serviceCollection = serviceCollection;
            this.projectName = projectName;
            this.containerBuilder = new ContainerBuilder();
        }

        public ServiceContainerBuilder UseConfiguration(IConfigurationRoot configurationRoot)
        {
            this.containerBuilder.RegisterInstance(configurationRoot);
            return this;
        }

        public ServiceContainerBuilder UseEnvironment(IHostingEnvironment hostingEnvironment)
        {
            this.containerBuilder.RegisterInstance(hostingEnvironment);
            return this;
        }

        public ServiceContainerBuilder UseDbContext<T>() where T : DbContext
        {
            this.containerBuilder
                .RegisterType<T>();
            return this;
        }

        public ServiceContainerBuilder UseCors(CorsPolicyConfiguration corsPolicy)
        {
            // add cors to allow cross site origin
            this.serviceCollection.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy(corsPolicy.Name, policyBuilder =>
                {
                    if(corsPolicy.AllowedOrigins == null)
                        policyBuilder.AllowAnyOrigin();
                    else
                        policyBuilder.WithOrigins(corsPolicy.AllowedOrigins);

                    if (corsPolicy.AllowedHeaders == null)
                        policyBuilder.AllowAnyHeader();
                    else
                        policyBuilder.WithHeaders(corsPolicy.AllowedHeaders);


                    if (corsPolicy.AllowedMethods == null)
                        policyBuilder.AllowAnyMethod();
                    else
                        policyBuilder.WithMethods(corsPolicy.AllowedMethods);

                    if(corsPolicy.AllowCredentials)
                        policyBuilder.AllowCredentials();
                });
            });

            if (corsPolicy.RegisterGlobally)
            {
                //// apply cors globally
                //// u can also apply them per controller / action
                //// see https://docs.microsoft.com/en-us/aspnet/core/security/cors
                //// for more information on this topic
                this.serviceCollection.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new CorsAuthorizationFilterFactory(corsPolicy.Name));
                });
            }

            return this;
        }

        public ServiceContainerBuilder UseCors(string corsPolicyName, Action<CorsPolicyBuilder> corsPolicyBuilder, bool registerGlobally = false)
        {
            // add cors to allow cross site origin
            this.serviceCollection.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy(corsPolicyName, corsPolicyBuilder);
            });

            if (registerGlobally)
            {
                //// apply cors globally
                //// u can also apply them per controller / action
                //// see https://docs.microsoft.com/en-us/aspnet/core/security/cors
                //// for more information on this topic
                this.serviceCollection.Configure<MvcOptions>(options => options.Filters.Add(new CorsAuthorizationFilterFactory(corsPolicyName)));
            }

            return this;
        }

        public ServiceContainerBuilder AddAutoMapper()
        {
            var assemblyNames = DependencyContext
                .Default
                .CompileLibraries
                .SelectMany(compileLib => compileLib.Assemblies)
                .Where(assemblyName => 
                    assemblyName.StartsWith(this.projectName, StringComparison.OrdinalIgnoreCase));



            // register the assemblies of type Profile
            // this will register all available mapping profiles
            foreach (var assemblyName in assemblyNames)
            {
                this.containerBuilder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName(assemblyName.Replace(".dll", ""))))
                    .Where(type => type.IsAssignableTo<Profile>())
                    .As<Profile>()
                    .SingleInstance();
            }

            // register all AutoMapper profiles
            // create the mapperconfiguration and register it as single instance
            this.containerBuilder.Register(componentContext =>
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
            this.containerBuilder.Register(componentContext =>
            {
                return componentContext
                    .Resolve<IComponentContext>()
                    .Resolve<MapperConfiguration>()
                    .CreateMapper(type => componentContext.Resolve<IComponentContext>()
                    .Resolve(type));
            })
            .As<IMapper>()
            .SingleInstance();

            this.containerBuilder.Register(componentContext => componentContext.Resolve<MapperConfiguration>()
                .CreateMapper(componentContext.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();

            return this;
        }

        public ServiceContainerBuilder AddEntityFramework<T>() where T : DbContext
        {
            //this.containerBuilder.RegisterType<T>();

            this.serviceCollection.AddEntityFramework()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<T>();

            return this;
        }

        public IContainer Build()
        {
            this.containerBuilder.Populate(this.serviceCollection);
            return this.containerBuilder.Build();
        }

        public ServiceContainerBuilder UseDefaultJSONOptions()
        {
            this.serviceCollection.AddMvc()
                .AddJsonOptions(optionsBuilder =>
                {
                    // make sure that the properties are resolved based on CamelCase-Rules
                    // returns lowercamelcase properties when sending data, when receiving data uppercamelcase properties
                    optionsBuilder.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    optionsBuilder.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            return this;
        }

        public ServiceContainerBuilder UseGenericRepositoryPattern<T>() where T : DbContext
        {    
            // register the DataRepository as generic
            // everytime an datarepository is called it will automatically be created with the necessary type
            // for instance IDataRepository<T> where T is of Type IPersistedData
            // resolve inner dependency of the repository
            this.containerBuilder.RegisterGeneric(typeof(EntityRepository<>))
                .As(typeof(IEntityRepository<>))
                .InstancePerRequest()
                .WithParameter(new ResolvedParameter(
                    (parameterInfo, componentContext) => parameterInfo.ParameterType == typeof(DbContext),
                    (parameterInfo, componentContext) => componentContext.Resolve<T>()))
                .InstancePerLifetimeScope();

            return this;
        }
    }
}
