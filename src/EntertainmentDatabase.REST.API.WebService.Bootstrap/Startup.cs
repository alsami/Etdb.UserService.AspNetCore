using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using EntertainmentDatabase.REST.API.ServiceBase.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using EntertainmentDatabase.REST.API.WebService.Data;
using EntertainmentDatabase.REST.API.WebService.Misc.Filters;

namespace EntertainmentDatabase.REST.API.WebService.Bootstrap
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfigurationRoot configurationRoot;

        private const string CorsName = "AllowAll";
        private readonly string logPath = $"Logs/{Assembly.GetEntryAssembly().GetName().Name}.log";
        private const string SeqPath = "http://localhost:5341";

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
                .WriteTo.Seq(Startup.SeqPath)
                .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddApiExplorer();

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
                    options.ApiName = "EntertainmentDatabase.REST.API.WebService";
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(RessourceNotFoundExceptionFilter));
                options.Filters.Add(typeof(ActionLogFilter));
                options.Filters.Add(typeof(ErrorLogFilter));
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddDbContext<EntertainmentDatabaseWebServiceContext>()
                .AddEntityFrameworkSqlServer();

            services.AddCors(options =>
            {
                options.AddPolicy(Startup.CorsName, policyOptions =>
                {
                    policyOptions.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                });
            });

            services.Configure<MvcOptions>(options => options.Filters.Add(new CorsAuthorizationFilterFactory(Startup.CorsName)));

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
                .UseGenericRepositoryPattern<EntertainmentDatabaseWebServiceContext>()
                .UseEnvironment(this.environment)
                .UseConfiguration(this.configurationRoot)
                .RegisterTypeAsSingleton<HttpContextAccessor, IHttpContextAccessor>();
        }
    }
}
