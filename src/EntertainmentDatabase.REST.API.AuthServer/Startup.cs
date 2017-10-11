using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using EntertainmentDatabase.REST.API.AuthServer.Config;
using Autofac;
using EntertainmentDatabase.REST.API.AuthServer.Validators;
using IdentityServer4.Validation;
using EntertainmentDatabase.REST.ServiceBase.Builder;
using EntertainmentDatabase.REST.API.DataAccess;

namespace EntertainmentDatabase.REST.API.AuthServer
{
    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfigurationRoot configurationRoot;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.configurationRoot = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(new IdentityResourceConfig().GetIdentityResource())
                .AddInMemoryApiResources(new ApiResourceConfig().GetApiResource())
                .AddInMemoryClients(new ClientConfig().GetClients());

            services.AddDbContext<EntertainmentDatabaseContext>()
                .AddEntityFrameworkSqlServer()
                .AddMvc();

            services.AddCors(options =>
            {
                options.AddPolicy("All", opt =>
                {
                    opt.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            })
            .Configure<MvcOptions>(options => options.Filters.Add(new CorsAuthorizationFilterFactory("All")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            new ServiceContainerBuilder(containerBuilder)
                //.AddDbContext<EntertainmentDatabaseContext>()
                .UseEnvironment(this.hostingEnvironment)
                .UseConfiguration(this.configurationRoot)
                .UseGenericRepositoryPattern<EntertainmentDatabaseContext>()
                .RegisterTypeAsSingleton<ResourceOwnerPasswordValidator, IResourceOwnerPasswordValidator>();
        }
    }
}
