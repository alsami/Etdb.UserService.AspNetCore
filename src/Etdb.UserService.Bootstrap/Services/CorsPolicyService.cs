using System;
using System.Linq;
using System.Threading.Tasks;
using Etdb.UserService.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Etdb.UserService.Bootstrap.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment environment;

        public CorsPolicyService(IConfiguration configuration, IHostingEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }
        
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            if (this.environment.IsDevelopment() || this.environment.IsLocalDevelopment())
            {
                return Task.FromResult(true);
            }

            var allowedOrigins = this.configuration
                .GetSection("IdentityConfig")
                .GetSection("Origins")
                .Get<string[]>();

            return Task.FromResult(allowedOrigins.Any(allowed =>
                allowed.Equals(origin, StringComparison.OrdinalIgnoreCase)));
        }
    }
}