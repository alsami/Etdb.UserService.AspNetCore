using Autofac;
using Etdb.ServiceBase.Builder.Builder;
using Etdb.ServiceBase.Constants;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.ServiceBase.ErrorHandling.Filters;
using Etdb.UserService.Application.Config;
using Etdb.UserService.Application.Services;
using Etdb.UserService.Application.Validators;
using Etdb.UserService.AutoMapper.Profiles;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Etdb.UserService.Bootstrap
{
    public class Startup
    {
        private const string SwaggerDocDescription = "Etdb " + ServiceNames.UserService + " V1";
        private const string SwaggerDocVersion = "v1";
        private const string SwaggerDocJsonUri = "/swagger/v1/swagger.json";

        private const string CorsPolicyName = "AllowAll";

        private const string AuthenticationSchema = "Bearer";
        private readonly IConfigurationRoot configurationRoot;
        private readonly IHostingEnvironment hostingEnvironment;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            this.configurationRoot = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
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

                    options.Filters.Add(typeof(UnhandledExceptionFilter));
                    options.Filters.Add(typeof(ConcurrencyExceptionFilter));
                    options.Filters.Add(typeof(AccessDeniedExceptionFilter));
                    options.Filters.Add(typeof(GeneralValidationExceptionFilter));
                    options.Filters.Add(typeof(ResourceNotFoundExceptionFilter));
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
                options.SwaggerDoc(SwaggerDocVersion, new Info
                {
                    Title = SwaggerDocDescription,
                    Version = SwaggerDocVersion
                });
            });

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(new IdentityResourceConfig().GetIdentityResource())
                .AddInMemoryApiResources(new ApiResourceConfig().GetApiResource())
                .AddInMemoryClients(new ClientConfig().GetClients(clientId, clientSecret, allowedOrigins));

            services.AddAuthentication(AuthenticationSchema)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = ServiceNames.UserService;
                });

            services.AddCors(options =>
                {
                    options.AddPolicy(CorsPolicyName, builder =>
                    {
                        builder.WithOrigins(allowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                })
                .Configure<MvcOptions>(options =>
                    options.Filters.Add(new CorsAuthorizationFilterFactory(CorsPolicyName)));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (this.hostingEnvironment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(action => { action.SwaggerEndpoint(SwaggerDocJsonUri, SwaggerDocDescription); });
            }

            app.UseIdentityServer();

            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            new ServiceContainerBuilder(containerBuilder)
                .UseCqrs()
                .UseAutoMapper(typeof(UsersProfile).Assembly)
                .UseEnvironment(this.hostingEnvironment)
                .UseConfiguration(this.configurationRoot)
                .RegisterTypeAsSingleton<Hasher, IHasher>()
                .RegisterTypePerLifetimeScope<ResourceOwnerPasswordValidator, IResourceOwnerPasswordValidator>()
                .RegisterTypePerLifetimeScope<ProfileService, IProfileService>();
        }
    }
}