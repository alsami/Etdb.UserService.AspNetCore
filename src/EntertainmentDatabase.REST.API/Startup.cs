using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using EntertainmentDatabase.Rest.DataAccess.Abstractions;
using EntertainmentDatabase.Rest.DataAccess.Facade;
using EntertainmentDatabase.REST.API.DatabaseContext;
using EntertainmentDatabase.REST.Infrastructure.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EntertainmentDatabase.REST.API
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfigurationRoot configurationRoot;
        private IContainer applicationContainer;
        private const string Production = "Production";
        private const string Development = "Development";

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
            var assemblies = new List<Assembly>();
            Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .Where(assemblyName => assemblyName.FullName.StartsWith("EntertainmentDatabase.REST"))
                .ToList()
                .ForEach(assemblyName => assemblies.Add(Assembly.Load(assemblyName)));

            var containerBuilder = new IoCBuilder(services, "EntertainmentDatabase.REST")
                .RegisterConfiguration(this.configurationRoot)
                .RegisterEnvironment(this.environment)
                .AddAutoMapper(assemblies)
                .AddEntityFramework(this.environment.IsDevelopment()
                    ? this.configurationRoot.GetConnectionString(Startup.Development)
                    : this.configurationRoot.GetConnectionString(Startup.Production))
                    .UseDefaultPersistency()
                .ConfigureCores("AllowAll", true)
                .UseDefaultJSONOptions();
            
            this.applicationContainer = containerBuilder.Build();

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
