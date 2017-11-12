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
using AutoMapper;
using EntertainmentDatabase.REST.API.Presentation.DataTransferObjects.Resolver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using EntertainmentDatabase.REST.ServiceBase.Generics.Facades;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using EntertainmentDatabase.REST.API.Presentation.DataTransferObjects.Mappings;
using Microsoft.Extensions.DependencyModel;
using System.Linq;

namespace EntertainmentDatabase.REST.API.Bootstrap
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfigurationRoot configurationRoot;

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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddApiExplorer()
                .AddAuthorization();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Entertainment-Database-REST",
                    Version = "v1"
                });
            });

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "EntertainmentDatabase.REST.API";
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(RessourceNotFoundExceptionFilter));
                options.Filters.Add(typeof(RegisterExceptionFilter));
                options.Filters.Add(typeof(LoginFailedExceptionFilter));
                options.Filters.Add(typeof(ActionLogFilter));
                options.Filters.Add(typeof(ErrorLogFilter));
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddDbContext<EntertainmentDatabaseContext>()
                .AddEntityFrameworkSqlServer();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policyOptions =>
                {
                    policyOptions.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                });
            });

            services.Configure<MvcOptions>(options => options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll")));

            services.AddAutoMapper(DependencyContext
                .Default
                .CompileLibraries
                .SelectMany(lib => lib.Assemblies)
                .Where(assemblyName => assemblyName.StartsWith("EntertainmentDatabase.REST.API"))
                .Select(assemblyName => Assembly.Load(assemblyName.Replace(".dll", ""))));
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            if (this.environment.IsDevelopment())
            {
                loggerFactory.AddConsole(this.configurationRoot.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            loggerFactory.AddSerilog();

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseSwagger();
            app.UseSwaggerUI(action =>
            {
                action.SwaggerEndpoint("/swagger/v1/swagger.json", "Entertainment-Database-REST V1");
            });

            app.UseAuthentication();

            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            new ServiceContainerBuilder(builder)
                .UseGenericRepositoryPattern<EntertainmentDatabaseContext>()
                .UseEnvironment(this.environment)
                .UseConfiguration(this.configurationRoot)
                .RegisterTypeAsSingleton<HttpContextAccessor, IHttpContextAccessor>();
        }
    }
}
