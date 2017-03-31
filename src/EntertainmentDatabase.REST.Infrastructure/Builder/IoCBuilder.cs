using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using AutoMapper.Configuration;
using EntertainmentDatabase.Rest.DataAccess.Abstraction;
using EntertainmentDatabase.Rest.DataAccess.Facade;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Reflection.Metadata;
using Remotion.Linq.Clauses;
using Microsoft.Extensions.DependencyModel;

namespace EntertainmentDatabase.REST.Infrastructure.Builder
{
    public class IoCBuilder
    {
        private readonly IServiceCollection serviceCollection;
        private readonly ContainerBuilder containerBuilder;
        private readonly string projectName;

        public IoCBuilder(IServiceCollection serviceCollection, string projectName)
        {
            this.serviceCollection = serviceCollection;
            this.projectName = projectName;
            this.containerBuilder = new ContainerBuilder();
        }

        public IoCBuilder RegisterConfiguration(IConfigurationRoot configurationRoot)
        {
            this.containerBuilder.RegisterInstance(configurationRoot);
            return this;
        }

        public IoCBuilder RegisterEnvironment(IHostingEnvironment hostingEnvironment)
        {
            this.containerBuilder.RegisterInstance(hostingEnvironment);
            return this;
        }

        // TODO: implement 
        public IoCBuilder ConfigureCores(string policyName, bool registerGlobally = false, string[] origins = null, HttpMethod[] methods = null)
        {
            // add cors to allow cross site origin
            this.serviceCollection.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy(policyName, policyBuilder =>
                {
                    // you can define special origin by using 
                    //policyBuilder.WithOrigins(origins);
                    policyBuilder.AllowAnyOrigin();
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                    policyBuilder.AllowCredentials();
                });
            });

            if (registerGlobally)
            {
                //// apply cors globally
                //// u can also apply them per controller / action
                //// see https://docs.microsoft.com/en-us/aspnet/core/security/cors
                //// for more information on this topic
                this.serviceCollection.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
                });
            }

            return this;
        }

        public IoCBuilder AddAutoMapper()
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

        public IoCBuilder AddEntityFramework(string connectionString)
        {
            
            this.serviceCollection.AddEntityFramework()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<DbContext>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseSqlServer(connectionString));

            return this;
        }

        public IContainer Build()
        {
            this.containerBuilder.Populate(this.serviceCollection);
            var container = this.containerBuilder.Build();

            this.containerBuilder.RegisterInstance(container)
                .As<IContainer>()
                .AsSelf()
                .AutoActivate();

            return container;
        }

        public IoCBuilder UseDefaultJSONOptions()
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

        /// <summary>
        /// Requires your entities to be implemented
        /// </summary>
        /// <returns></returns>
        public IoCBuilder UseGenericRepositoryPattern()
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
                    (parameterInfo, componentContext) => componentContext.Resolve<DbContext>()))
                .InstancePerLifetimeScope();

            return this;
        }
    }
}
