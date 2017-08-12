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

namespace EntertainmentDatabase.REST.API.Bootstrap
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfigurationRoot configurationRoot;
        private IContainer applicationContainer;
        private readonly Stopwatch stopWatch;

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
                .WriteTo.RollingFile($"Logs/{Assembly.GetEntryAssembly().GetName().Name}.log")
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            this.stopWatch = new Stopwatch();
            stopWatch.Start();
            Log.Information("Starting Service");
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Log.Information("Configuring Service Dependencies");
            Log.Information($"Elapsed time {stopWatch.ElapsedMilliseconds.ToString()}");

            var containerBuilder = new ServiceContainerBuilder(services, "EntertainmentDatabase.REST")
                .AddCoreServiceRequirement(mvcOptionsAction =>
                    {
                        mvcOptionsAction.Filters.Add(typeof(RessourceNotFoundFilter));
                        mvcOptionsAction.Filters.Add(typeof(ActionLogFilter));
                        mvcOptionsAction.Filters.Add(typeof(ErrorLogFilter));
                    }, 
                    options => {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
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
                .RegisterTypeAsSingleton<HttpContextAccessor, IHttpContextAccessor>();


            services.AddSwaggerGen(action =>
            {
                action.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            this.applicationContainer = containerBuilder.Build();


            Log.Information("Configuring service dependencies done");
            Log.Information($"Elapsed time {stopWatch.ElapsedMilliseconds.ToString()}");

            return new AutofacServiceProvider(this.applicationContainer);
        }


        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            if (this.environment.IsDevelopment())
            {
                loggerFactory.AddConsole(this.configurationRoot.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            //loggerFactory.AddFile($"Logs/{Assembly.GetEntryAssembly().GetName().Name}.log");
            loggerFactory.AddSerilog();

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(action =>
            {
                action.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.Run(async (context) => {
            //    await context.Response.WriteAsync("<h1>Service running!</h1>");
            //});
        }
    }
}
