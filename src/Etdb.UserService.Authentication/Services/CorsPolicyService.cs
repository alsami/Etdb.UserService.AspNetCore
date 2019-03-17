using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Etdb.UserService.Authentication.Configuration;
using Etdb.UserService.Extensions;

namespace Etdb.UserService.Authentication.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IHostingEnvironment environment;
        private readonly IOptions<AllowedOriginsConfiguration> options;

        public CorsPolicyService(IHostingEnvironment environment, IOptions<AllowedOriginsConfiguration> options)
        {
            this.environment = environment;
            this.options = options;
        }

        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            if (this.environment.IsDevelopment() || this.environment.IsLocalDevelopment())
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(this.options.Value.Origins.Any(allowed =>
                allowed.Equals(origin, StringComparison.OrdinalIgnoreCase)));
        }
    }
}