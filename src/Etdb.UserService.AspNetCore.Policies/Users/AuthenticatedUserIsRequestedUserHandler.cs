using System.Threading.Tasks;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Etdb.UserService.Policies.AspNetCore.Users
{
    public class AuthenticatedUserIsRequestedUserHandler : IAuthorizationHandler
    {
        private readonly IApplicationUser applicationUser;

        public AuthenticatedUserIsRequestedUserHandler(IApplicationUser applicationUser)
        {
            this.applicationUser = applicationUser;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var authorizationFilterContext = (AuthorizationFilterContext) context.Resource;

            if (!authorizationFilterContext.HttpContext.TryParseRouteParameterId("userId",
                    out var userId) &&
                !authorizationFilterContext.HttpContext.TryParseRouteParameterId("id", out userId))
                return Task.CompletedTask;

            if (this.applicationUser.Id != userId)
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}