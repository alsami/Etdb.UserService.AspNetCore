using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class UserUrlFactory : IUserUrlFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public UserUrlFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        public string GenerateUrl(User user, string route)
        {
            var url = this.linkGenerator.GetUriByName(this.httpContextAccessor.HttpContext, route, new
            {
                userId = user.Id,
            });

            return url;
        }
    }
}