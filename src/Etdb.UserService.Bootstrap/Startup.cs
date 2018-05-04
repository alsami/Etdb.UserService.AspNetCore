using Autofac;
using Etdb.ServiceBase.Builder.Builder;
using Etdb.ServiceBase.Constants;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.ErrorHandling.Filters;
using Etdb.UserService.Application.Config;
using Etdb.UserService.Application.Services;
using Etdb.UserService.Application.Validators;
using Etdb.UserService.AutoMapper.Profiles;
using Etdb.UserService.Cqrs.Handler;
using Etdb.UserService.Repositories;
using Etdb.UserService.Repositories.Context;
using Etdb.UserService.Services.Abstractions;
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
        private readonly IConfigurationRoot configuration;
        private readonly IHostingEnvironment environment;
        
        private const string SwaggerDocDescription = "Etdb " + ServiceNames.UserService + " V1";
        private const string SwaggerDocVersion = "v1";
        private const string SwaggerDocJsonUri = "/swagger/v1/swagger.json";
        private const string CorsPolicyName = "AllowAll";
        private const string AuthenticationSchema = "Bearer";
        private const string DbContextOptions = "DocumentDbContextOptions";

        public Startup(IHostingEnvironment environment)
        {
            this.environment = environment;

            this.configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var identityConfig = this.configuration
                .GetSection("IdentityConfig");

            var allowedOrigins = identityConfig
                .GetSection("Origins")
                .Get<string[]>();

            var clientId = identityConfig
                .GetSection("WebClient")
                .GetValue<string>("Name");

            var clientSecret = identityConfig
                .GetSection("WebClient")
                .GetValue<string>("Secret");

            var authority = identityConfig
                .GetValue<string>("Authority");

            services.AddMvc(options =>
                {
                    options.OutputFormatters.RemoveType<XmlSerializerOutputFormatter>();
                    options.InputFormatters.RemoveType<XmlSerializerInputFormatter>();

                    var requireAuthenticatedUserPolicy = new AuthorizeFilter(new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build());
                    
                    options.Filters.Add(requireAuthenticatedUserPolicy);

                    options.Filters.Add(typeof(UnhandledExceptionFilter));
                    options.Filters.Add(typeof(ConcurrencyExceptionFilter));
                    options.Filters.Add(typeof(AccessDeniedExceptionFilter));
                    options.Filters.Add(typeof(GeneralValidationExceptionFilter));
                    options.Filters.Add(typeof(ResourceNotFoundExceptionFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            if (this.environment.IsDevelopment())
            {
                services.AddMvcCore()
                    .AddApiExplorer();

                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(Startup.SwaggerDocVersion, new Info
                    {
                        Title = SwaggerDocDescription,
                        Version = SwaggerDocVersion
                    });
                });   
            }

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(new IdentityResourceConfig().GetIdentityResource())
                .AddInMemoryApiResources(new ApiResourceConfig().GetApiResource())
                .AddInMemoryClients(new ClientConfig().GetClients(clientId, clientSecret, allowedOrigins));

            services.AddAuthentication(AuthenticationSchema)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = this.environment.IsProduction();
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

            services.Configure<DocumentDbContextOptions>(options =>
            {
                this.configuration.GetSection(Startup.DbContextOptions)
                    .Bind(options);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (this.environment.IsDevelopment())
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
                .UseCqrs(typeof(UserRegisterCommandHandler).Assembly)
                .UseAutoMapper(typeof(UsersProfile).Assembly)
                .UseEnvironment(this.environment)
                .UseConfiguration(this.configuration)
                .UseGenericDocumentRepositoryPattern<UserServiceDbContext>(typeof(UserRepository).Assembly)
                .RegisterTypeAsSingleton<Hasher, IHasher>()
                .RegisterTypePerLifetimeScope<ResourceOwnerPasswordValidator, IResourceOwnerPasswordValidator>()
                .RegisterTypePerLifetimeScope<ProfileService, IProfileService>()
                .RegisterTypePerLifetimeScope<Services.UserService, IUserService>();
        }
    }
}