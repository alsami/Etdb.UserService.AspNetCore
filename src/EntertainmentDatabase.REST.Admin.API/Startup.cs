using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using EntertainmentDatabase.REST.API.Context;
using EntertainmentDatabase.REST.ServiceBase.Builder;
using EntertainmentDatabase.REST.ServiceBase.Configuration;
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

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var containerBuilder = new ServiceContainerBuilder(services, "EntertainmentDatabase.REST")
                .AddAutoMapper()
                .AddEntityFramework<EntertainmentDatabaseContext>()
                .UseConfiguration(this.configurationRoot)
                .UseEnvironment(this.environment)
                .UseGenericRepositoryPattern<EntertainmentDatabaseContext>()
                .UseCors("AllowAll", builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                }, true)
                .UseDefaultJSONOptions();

            this.applicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(this.applicationContainer);
        }


        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.configurationRoot.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (this.environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseMvc();
        }
    }
}
