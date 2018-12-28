﻿using System.IO;
using System.IO.Compression;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Etdb.ServiceBase.Filter;
using Etdb.UserService.Authentication.Configs;
using Etdb.UserService.Bootstrap.Config;
using Etdb.UserService.Constants;
using Etdb.UserService.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Etdb.UserService.Bootstrap.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        private const string FilesLocalSubpath = "Files";
        private static string CookieName =    typeof(Startup).Assembly.GetName().Name.Replace(".dll", "").Replace(".exe", "");


        public static IServiceCollection ConfigureCompression(this IServiceCollection services,
            CompressionLevel level = CompressionLevel.Optimal)
        {
            return services
                .Configure<GzipCompressionProviderOptions>(options => options.Level = level)
                .AddResponseCompression();
        }

        public static IServiceCollection ConfigureFileStoreOptions(this IServiceCollection services,
            IConfiguration configuration, IHostingEnvironment environment)
        {
            return services.Configure<FileStoreOptions>(options =>
            {
                if (environment.IsDevelopment() || environment.IsLocalDevelopment())
                {
                    options.ImagePath = Path.Combine(environment.ContentRootPath,
                        ServiceCollectionExtensions.FilesLocalSubpath);

                    return;
                }

                configuration.GetSection(nameof(FileStoreOptions)).Bind(options);
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

        public static IServiceCollection ConfigureRedisCache(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddDistributedRedisCache(options => configuration
                .GetSection(nameof(RedisCacheOptions))
                .Bind(options));
        }

        public static IServiceCollection ConfigureAllowedOriginsOptions(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<AllowedOriginsOptions>(options => configuration
                .GetSection(nameof(AllowedOriginsOptions))
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

            // TODO use new CompatibilityVersion
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-2.2
            // https://stackoverflow.com/questions/46096068/asp-net-core-2-0-creating-urlhelper-without-request
            // services.AddRouting();
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
            services.AddIdentityServer(options => options.Authentication.CookieAuthenticationScheme = ServiceCollectionExtensions.CookieName)
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityResourceConfig.GetIdentityResource())
                .AddInMemoryApiResources(ApiResourceConfig.GetApiResource())
                .AddInMemoryClients(ClientConfig.GetClients(clientId, clientSecret, allowedOrigins));

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