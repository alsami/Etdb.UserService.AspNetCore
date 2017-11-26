using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using ETDB.API.ServiceBase.Abstractions.Hasher;
using ETDB.API.ServiceBase.Builder;
using ETDB.API.ServiceBase.Constants;
using ETDB.API.ServiceBase.Hasher;
using ETDB.API.UserService.Application.Config;
using ETDB.API.UserService.Application.ExceptionFilter;
using ETDB.API.UserService.Application.Services;
using ETDB.API.UserService.Application.Validators;
using ETDB.API.UserService.Data;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using ETDB.API.UserService.EventStore;
using ETDB.API.UserService.Repositories;
using ETDB.API.UserService.Repositories.Repositories;

namespace ETDB.API.UserService.Bootstrap
{
    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfigurationRoot configurationRoot;

        private const string SwaggerDocDescription = "ETDB " + ServiceNames.UserService + " V1";
        private const string SwaggerDocVersion = "v1";
        private const string SwaggerDocJsonUri = "/swagger/v1/swagger.json";

        private const string CorsPolicyName = "AllowAll";

        private const string AuthenticationSchema = "Bearer";

        private const string ApplicationAssemblyPrefix = "ETDB.API.UserService";

        private const string MediatorAssemblyPrefix = "ETDB.API.ServiceBase";

        private readonly Assembly[] applicatiAssemblies;

        private readonly Assembly[] mediatorAssemblies;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            this.configurationRoot = builder.Build();

            this.applicatiAssemblies = DependencyContext
                .Default
                .CompileLibraries
                .SelectMany(lib => lib.Assemblies)
                .Where(assemblyName => assemblyName.StartsWith(Startup.ApplicationAssemblyPrefix))
                .Select(assemblyName => Assembly.Load(assemblyName.Replace(".dll", "")))
                .ToArray();

            this.mediatorAssemblies = DependencyContext
                .Default
                .CompileLibraries
                .SelectMany(lib => lib.Assemblies)
                .Where(assemblyName => assemblyName.StartsWith(Startup.MediatorAssemblyPrefix))
                .Select(assemblyName => Assembly.Load(assemblyName.Replace(".dll", "")))
                .ToArray();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build()));

                options.Filters.Add(new DbUpdateExceptionFilter());
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddMvcCore()
                .AddApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Startup.SwaggerDocVersion, new Info
                {
                    Title = Startup.SwaggerDocDescription,
                    Version = Startup.SwaggerDocVersion
                });
            });

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(new IdentityResourceConfig().GetIdentityResource())
                .AddInMemoryApiResources(new ApiResourceConfig().GetApiResource())
                .AddInMemoryClients(new ClientConfig().GetClients(this.configurationRoot));

            services.AddAuthentication(Startup.AuthenticationSchema)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = ServiceNames.UserService;
                });

            services.AddCors(options =>
            {
                options.AddPolicy(Startup.CorsPolicyName, opt =>
                {
                    opt.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            })
            .Configure<MvcOptions>(options => 
                options.Filters.Add(new CorsAuthorizationFilterFactory(Startup.CorsPolicyName)));

            services.AddMediatR(this.applicatiAssemblies);

            services.AddAutoMapper(this.applicatiAssemblies);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(action =>
            {
                action.SwaggerEndpoint(Startup.SwaggerDocJsonUri, Startup.SwaggerDocDescription);
            });

            app.UseIdentityServer();
            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            new ServiceContainerBuilder(containerBuilder)
                .UseEnvironment(this.hostingEnvironment)
                .UseConfiguration(this.configurationRoot)
                .UseGenericRepositoryPattern<UserServiceContext>()
                .UseEventSourcing<UserServiceContext, EventStoreContext>(this.applicatiAssemblies)
                .RegisterTypePerDependency<Hasher, IHasher>()
                .RegisterTypePerLifetimeScope<ResourceOwnerPasswordValidator, IResourceOwnerPasswordValidator>()
                .RegisterTypePerLifetimeScope<ProfileService, IProfileService>()
                .RegisterTypePerLifetimeScope<UserRepository, IUserRepository>()
                .RegisterTypePerLifetimeScope<UserAppService, IUserAppService>();
        }
    }
}
