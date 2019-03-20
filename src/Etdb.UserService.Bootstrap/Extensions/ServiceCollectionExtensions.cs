using System.IO;
using System.IO.Compression;
using Etdb.ServiceBase.Constants;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Etdb.ServiceBase.Filter;
using Etdb.UserService.Authentication.Configuration;
using Etdb.UserService.Bootstrap.Configuration;
using Etdb.UserService.Constants;
using Etdb.UserService.Extensions;
using Etdb.UserService.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Etdb.UserService.Bootstrap.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        private const string FilesLocalSubpath = "Files";

        private static readonly string CookieName =
            typeof(Startup).Assembly.GetName().Name.Replace(".dll", "").Replace(".exe", "");


        public static IServiceCollection ConfigureCompression(this IServiceCollection services,
            CompressionLevel level = CompressionLevel.Optimal)
        {
            return services
                .Configure<GzipCompressionProviderOptions>(options => options.Level = level)
                .AddResponseCompression(options =>
                {
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                    options.MimeTypes = new[]
                    {
                        "application/json",
                        "application/json; charset=utf-8",
                        "image/*",
                        "application/octet"
                    };
                });
        }

        public static IServiceCollection ConfigureFileStoreOptions(this IServiceCollection services,
            IConfiguration configuration, IHostingEnvironment environment)
        {
            return services.Configure<FilestoreConfiguration>(options =>
            {
                if (environment.IsDevelopment() || environment.IsLocalDevelopment())
                {
                    options.ImagePath = Path.Combine(environment.ContentRootPath,
                        ServiceCollectionExtensions.FilesLocalSubpath);

                    return;
                }

                configuration.GetSection(nameof(FilestoreConfiguration)).Bind(options);
            });
        }

        public static IServiceCollection ConfigureIdentityServerOptions(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<IdentityServerConfiguration>(options =>
            {
                configuration.GetSection(nameof(IdentityServerConfiguration)).Bind(options);
            });
        }

        public static IServiceCollection ConfigureDocumentDbContextOptions(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<DocumentDbContextOptions>(options => configuration
                .GetSection(nameof(DocumentDbContextOptions))
                .Bind(options));
        }

        public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services,
            IHostingEnvironment environment, Info info, string title)
        {
            if (!environment.IsDevelopment() || environment.IsLocalDevelopment())
            {
                return services;
            }

            services.AddMvcCore()
                .AddApiExplorer();

            return services.AddSwaggerGen(options => options.SwaggerDoc(title, info));
        }

        public static IServiceCollection ConfigureDistributedCaching(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RedisConfiguration>(options => configuration
                .GetSection(nameof(RedisConfiguration))
                .Bind(options));


            return services.AddDistributedRedisCache(options =>
            {
                options.Configuration = services.BuildServiceProvider()
                    .GetRequiredService<IOptions<RedisConfiguration>>().Value.Connection;
                options.InstanceName = $"{ServiceNames.UserService}_";
            });
        }

        public static IServiceCollection ConfigureAllowedOriginsOptions(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<AllowedOriginsConfiguration>(options => configuration
                .GetSection(nameof(AllowedOriginsConfiguration))
                .Bind(options));
        }

        public static IServiceCollection ConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.OutputFormatters.RemoveType<XmlSerializerOutputFormatter>();
                    options.InputFormatters.RemoveType<XmlSerializerInputFormatter>();

                    var requireAuthenticatedUserPolicy = new AuthorizeFilter(new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build());

                    options.Filters.Add(requireAuthenticatedUserPolicy);
                    options.Filters.Add(typeof(UnhandledExceptionFilter));
                    options.Filters.Add(typeof(AccessDeniedExceptionFilter));
                    options.Filters.Add(typeof(GeneralValidationExceptionFilter));
                    options.Filters.Add(typeof(ResourceLockedExceptionFilter));
                    options.Filters.Add(typeof(ResourceNotFoundExceptionFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
        {
            return services.AddHttpClient();
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services,
            IHostingEnvironment environment, string[] allowedOrigins, string policyName)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod();

                    if (environment.IsDevelopment() || environment.IsLocalDevelopment())
                    {
                        builder.AllowAnyOrigin();
                        return;
                    }

                    builder.WithOrigins(allowedOrigins);
                });
            });
        }

        public static IServiceCollection ConfigureIdentityServerAuthorization(this IServiceCollection services,
            string[] allowedOrigins, string clientId, string clientSecret)
        {
            services.AddIdentityServer(options =>
                    options.Authentication.CookieAuthenticationScheme = ServiceCollectionExtensions.CookieName)
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityResourceConfiguration.GetIdentityResource())
                .AddInMemoryApiResources(ApiResourceConfiguration.GetApiResource())
                .AddInMemoryClients(ClientConfiguration.GetClients(clientId, clientSecret, allowedOrigins));

            return services;
        }

        public static IServiceCollection ConfigureIdentityServerAuthentication(this IServiceCollection services,
            IHostingEnvironment environment, string schema, string apiName, string authority)
        {
            services.AddAuthentication(schema)
                .AddCookie(ServiceCollectionExtensions.CookieName)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = environment.IsProduction();
                    options.ApiName = apiName;
                });

            return services;
        }

        public static IServiceCollection ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            return services.AddAuthorization(options =>
            {
                options.AddPolicy(RolePolicyNames.AdminPolicy, builder => builder.RequireRole(RoleNames.Admin));
            });
        }
    }
}