using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using ETDB.API.ServiceBase.Abstractions.Hasher;
using ETDB.API.ServiceBase.Builder;
using ETDB.API.ServiceBase.Constants;
using ETDB.API.ServiceBase.Hasher;
using ETDB.API.UserService.Bootstrap.Config;
using ETDB.API.UserService.Bootstrap.Services;
using ETDB.API.UserService.Bootstrap.Validators;
using ETDB.API.UserService.Data;
using ETDB.API.UserService.Repositories;
using ETDB.API.UserService.Repositories.Base;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Swashbuckle.AspNetCore.Swagger;

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

        private const string AssemblyPrefix = "ETDB.API.UserService";

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            this.configurationRoot = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build()));
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

            services.AddDbContext<UserServiceContext>()
                .AddEntityFrameworkSqlServer();

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

            services.AddAutoMapper(DependencyContext
                .Default
                .CompileLibraries
                .SelectMany(lib => lib.Assemblies)
                .Where(assemblyName => assemblyName.StartsWith(Startup.AssemblyPrefix))
                .Select(assemblyName => Assembly.Load(assemblyName.Replace(".dll", ""))));
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
                .RegisterTypeAsSingleton<Hasher, IHasher>()
                .RegisterTypePerDependency<ResourceOwnerPasswordValidator, IResourceOwnerPasswordValidator>()
                .RegisterTypePerDependency<ProfileService, IProfileService>()
                .RegisterTypePerDependency<UserClaimsRepository, IUserClaimsRepository>();
        }
    }
}
