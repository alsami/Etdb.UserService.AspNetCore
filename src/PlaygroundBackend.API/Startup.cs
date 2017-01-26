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
using AutoMapper.QueryableExtensions.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using PlaygroundBackend.Infrastructure.Abstractions;
using PlaygroundBackend.Model.Mappings;
using PlaygroundBackend.Persistency;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PlaygroundBackend.Model.Modules;

namespace PlaygroundBackend.API
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfigurationRoot configurationRoot;
        private IContainer applicationContainer;
        private const string FirstPartOfProjectAssemblyName = "PlaygroundBackend";

        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.configurationRoot = builder.Build();

            this.environment = environment;
        }

        public IContainer ApplicationContainer { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
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

            //// apply cors globally
            //// u can also apply them per controller / action
            //// see https://docs.microsoft.com/en-us/aspnet/core/security/cors
            //// for more information on this topic
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            });

            // add entityframework and register the context
            // based on the environment we'll use different database connections
            services.AddEntityFramework()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<PlaygroundContext>();
            services.AddMvc()
                .AddJsonOptions(optionsBuilder =>
                {
                    // make sure that the properties are resolved based on CamelCase-Rules
                    // returns lowercamelcase properties when sending data, when receiving data uppercamelcase properties
                    optionsBuilder.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    optionsBuilder.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            // build the application container for dependencies
            var builder = new ContainerBuilder();

            // register custom modules
            builder.RegisterModule(new AutoMapperModule());

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
            builder.RegisterInstance(this.configurationRoot);
            builder.RegisterInstance(this.environment);

            // populate the service and build the ApplicationContainer
            builder.Populate(services);
            this.applicationContainer = builder.Build();

            // return the IoC Container
            return new AutofacServiceProvider(this.applicationContainer);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.configurationRoot.GetSection("Logging"));
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
