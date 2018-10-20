using System;
using System.Linq;
using System.Threading.Tasks;
using Etdb.UserService.Bootstrap.Config;
using Etdb.UserService.Bootstrap.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Bootstrap.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IHostingEnvironment environment;
        private readonly IOptions<AllowedOriginsOptions> options;

        public CorsPolicyService(IHostingEnvironment environment, IOptions<AllowedOriginsOptions> options)
        {
            this.environment = environment;
            this.options = options;
        }

        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            if (this.environment.IsDevelopment() || this.environment.IsLocalDevelopment()) return Task.FromResult(true);

            return Task.FromResult(this.options.Value.Origins.Any(allowed =>
                allowed.Equals(origin, StringComparison.OrdinalIgnoreCase)));
        }
    }
}