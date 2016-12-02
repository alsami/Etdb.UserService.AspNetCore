using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlaygroundBackend.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PlaygroundBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using PlaygroundBackend.Infrastructure.Abstractions;

namespace PlaygroundBackend.API
{
    public class Startup
    {
        private IHostingEnvironment env;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            this.env = env;
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
                .AddDbContext<PlaygroundContext>(options =>
                {
                    if (this.env.IsDevelopment())
                        options.UseSqlServer(this.Configuration.GetConnectionString("Development"));
                    else
                        options.UseSqlServer(this.Configuration.GetConnectionString("Production"));
                });
            services.AddMvc();

            // build the application container for dependencies
            var builder = new ContainerBuilder();

            // register the DataRepository as generic
            // everytime an datarepository is called it will automatically be created with the necessary type
            // for instance IDataRepository<T> where T is of Type IPersistedData
            builder.RegisterGeneric(typeof(DataRepository<>))
                .As(typeof(IDataRepository<>))
                .InstancePerRequest();
            this.ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
