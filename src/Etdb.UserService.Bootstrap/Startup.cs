using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Etdb.ServiceBase.Builder.Builder;
using Etdb.ServiceBase.Constants;
using Etdb.ServiceBase.General.Abstractions.Filters;
using Etdb.ServiceBase.General.Abstractions.Hasher;
using Etdb.ServiceBase.General.Hasher;
using Etdb.UserService.Application.Config;
using Etdb.UserService.Application.ExceptionFilter;
using Etdb.UserService.Application.Services;
using Etdb.UserService.Application.Validators;
using Etdb.UserService.Data;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Etdb.UserService.Bootstrap
{
    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfigurationRoot configurationRoot;

        private const string SwaggerDocDescription = "Etdb " + ServiceNames.UserService + " V1";
        private const string SwaggerDocVersion = "v1";
        private const string SwaggerDocJsonUri = "/swagger/v1/swagger.json";

        private const string CorsPolicyName = "AllowAll";

        private const string AuthenticationSchema = "Bearer";

        private const string ApplicationAssemblyPrefix = "Etdb.UserService";

        private readonly Assembly[] applicationAssemblies;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            this.configurationRoot = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();

            this.applicationAssemblies = DependencyContext
                .Default
                .CompileLibraries
                .SelectMany(lib => lib.Assemblies)
                .Where(assemblyName => assemblyName.StartsWith(Startup.ApplicationAssemblyPrefix))
                .Select(assemblyName => Assembly.Load(assemblyName.Replace(".dll", "")))
                .ToArray();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var allowedOrigins = this.configurationRoot
                .GetSection("IdentityConfig")
                .GetSection("Origins")
                .Get<string[]>();

            var clientId = this.configurationRoot
                .GetSection("IdentityConfig")
                .GetSection("WebClient")
                .GetValue<string>("Name");

            var clientSecret = this.configurationRoot
                .GetSection("IdentityConfig:WebClient")
                .GetValue<string>("Secret");

            services.AddMvc(options =>
            {
                options.OutputFormatters.RemoveType<XmlSerializerOutputFormatter>();
                options.InputFormatters.RemoveType<XmlSerializerInputFormatter>();

                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build()));

                options.Filters.Add(typeof(GeneralExceptionFilter));
                options.Filters.Add(typeof(CommandValidationExceptionFilter));
                options.Filters.Add(typeof(ModelStateValidationExceptionFilter));
                options.Filters.Add(typeof(ConcurrencyExceptionFilter));
            })
            .AddJsonOptions(options =>
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
                .AddInMemoryClients(new ClientConfig().GetClients(clientId, clientSecret, allowedOrigins));

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
                    opt.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            })
            .Configure<MvcOptions>(options => 
                options.Filters.Add(new CorsAuthorizationFilterFactory(Startup.CorsPolicyName)));

            services.AddMediatR(this.applicationAssemblies);

            services.AddAutoMapper(this.applicationAssemblies);
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
                .UseEventSourcing<UserServiceContext, EventStoreContext>(this.applicationAssemblies)
                .UseGenericRepositoryPattern<UserServiceContext>(this.applicationAssemblies)
                .UseEnvironment(this.hostingEnvironment)
                .UseConfiguration(this.configurationRoot)
                .RegisterTypeAsSingleton<Hasher, IHasher>()
                .RegisterTypePerLifetimeScope<ResourceOwnerPasswordValidator, IResourceOwnerPasswordValidator>()
                .RegisterTypePerLifetimeScope<ProfileService, IProfileService>();
        }
    }
}
