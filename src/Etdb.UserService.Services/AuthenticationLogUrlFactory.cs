using System;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class AuthenticationLogUrlFactory : IAuthenticationLogUrlFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public AuthenticationLogUrlFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        public string GenerateLoadAllUrl(Guid userId)
        {
            var url = this.linkGenerator.GetUriByName(this.httpContextAccessor.HttpContext,
                RouteNames.AuthenticationLogs.LoadAllRoute, new
                {
                    userId
                });

            return url;
        }
    }
}