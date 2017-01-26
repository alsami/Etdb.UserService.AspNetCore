using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using PlaygroundBackend.Infrastructure.Abstractions;
using PlaygroundBackend.Model.Mappings;
using PlaygroundBackend.Persistency;
using Microsoft.Extensions.DependencyModel;

namespace PlaygroundBackend.API
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private const string FirstPartOfProjectAssemblyName = "PlaygroundBackend";

        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            this.environment = environment;
        }

        public IConfigurationRoot Configuration { get; }

        public IContainer ApplicationContainer { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // add entityframework and register the context
            // based on the environment we'll use different database connections
            services.AddEntityFramework()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<PlaygroundContext>();
            services.AddMvc();

            // add cors to allow cross site origin
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("AllowAll", policyBuilder =>
                {
                    // you can define special origin by using 
                    //.WithOrigins("Origin1", "Origin2") and so on
                    policyBuilder.AllowAnyOrigin();
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                    policyBuilder.AllowCredentials();
                });
            });

            // apply cors globally
            // u can also apply them per controller / action
            // see https://docs.microsoft.com/en-us/aspnet/core/security/cors
            // for more information on this topic
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            });

            // build the application container for dependencies
            var builder = new ContainerBuilder();

            // load all referenced libraries
            // that are part of this project
            var assemblies = new List<Assembly>();
            Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .Where(assemblyName => assemblyName.FullName.StartsWith(Startup.FirstPartOfProjectAssemblyName))
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

            // create the mapping config and register it as single instance
            builder.Register(componentContext => new MapperConfiguration(config =>
                {
                    foreach (var profile in componentContext.Resolve<IEnumerable<Profile>>())
                    {
                        config.AddProfile(profile);
                    }
                }))
                .AsSelf()
                .SingleInstance();

            // resolve the created mapper configuration once for the life time of the instance
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>()
                .CreateMapper(ctx.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();


            // register the DataRepository as generic
            // everytime an datarepository is called it will automatically be created with the necessary type
            // for instance IDataRepository<T> where T is of Type IPersistedData
            // resolve inner dependency of the repository
            builder.RegisterGeneric(typeof(DataRepository<>))
                .As(typeof(IDataRepository<>))
                .InstancePerRequest()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(DbContext),
                    (pi, ctx) => ctx.Resolve<PlaygroundContext>()))
                .InstancePerLifetimeScope();

            // register configuration and environment to make it injectable for other libraries
            builder.RegisterInstance(this.Configuration);
            builder.RegisterInstance(this.environment);

            // populate the service and build the ApplicationContainer
            builder.Populate(services);
            this.ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (this.environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseMvc();
        }
    }
}
