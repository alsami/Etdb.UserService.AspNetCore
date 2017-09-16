using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EntertainmentDatabase.REST.API.DataAccess;
using EntertainmentDatabase.REST.ServiceBase.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Serilog.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle;
using System.Threading.Tasks;
using System.Diagnostics;
using EntertainmentDatabase.REST.API.Misc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using EntertainmentDatabase.REST.API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace EntertainmentDatabase.REST.API.Bootstrap
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfigurationRoot configurationRoot;
        private IContainer applicationContainer;

        private const string CorsName = "AllowAll";
        private readonly string logPath = $"Logs/{Assembly.GetEntryAssembly().GetName().Name}.log";
        private readonly string seqPath = "http://localhost:5341";

        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.configurationRoot = builder.Build();

            this.environment = environment;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(this.logPath)
                .WriteTo.Seq(this.seqPath)
                .CreateLogger();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var containerBuilder = new ServiceContainerBuilder(services, "EntertainmentDatabase.REST")
                .AddCoreServiceRequirement(mvcOptionsAction =>
                {
                    mvcOptionsAction.Filters.Add(typeof(RessourceNotFoundExceptionFilter));
                    mvcOptionsAction.Filters.Add(typeof(RegisterExceptionFilter));
                    mvcOptionsAction.Filters.Add(typeof(LoginFailedExceptionFilter));
                    mvcOptionsAction.Filters.Add(typeof(ActionLogFilter));
                    mvcOptionsAction.Filters.Add(typeof(ErrorLogFilter));
                }, 
                    options => {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddAutoMapper()
                .AddDbContext<EntertainmentDatabaseContext>()
                .AddSwaggerGen(action =>
                {
                    action.SwaggerDoc("v1", new Info
                    {
                        Title = "Entertainment-Database-REST",
                        Version = "v1"
                    });
                })
                .UseConfiguration(this.configurationRoot)
                .UseEnvironment(this.environment)
                .UseGenericRepositoryPattern<EntertainmentDatabaseContext>()
                .UseCors(Startup.CorsName, builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                }, true)
                .RegisterTypeAsSingleton<DataSeeder>()
                .RegisterTypeAsSingleton<HttpContextAccessor, IHttpContextAccessor>();

            this.applicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(this.applicationContainer);
        }


        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, DataSeeder seeder)
        {
            if (this.environment.IsDevelopment())
            {
                loggerFactory.AddConsole(this.configurationRoot.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // causes error when applying db update
                // uncommenting helps for now
                // seeder.SeedDatabase().Wait();
            }
            loggerFactory.AddSerilog();

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseSwagger();
            app.UseSwaggerUI(action =>
            {
                action.SwaggerEndpoint("/swagger/v1/swagger.json", "Entertainment-Database-REST V1");
            });

            app.UseMvc();


            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("<h1>Service running!</h1>");
            });
        }
    }
}
