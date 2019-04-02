using System.Linq;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class ProfileImageUrlFactory : IProfileImageUrlFactory
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ContextLessRouteProvider contextLessRouteProvider;

        public ProfileImageUrlFactory(IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor,
            ContextLessRouteProvider contextLessRouteProvider)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
            this.httpContextAccessor = httpContextAccessor;
            this.contextLessRouteProvider = contextLessRouteProvider;
        }

        public string GenerateUrl(User user)
        {
            var context = this.actionContextAccessor.ActionContext ??
                          new ActionContext(this.httpContextAccessor.HttpContext,
                              new RouteData
                              {
                                  Routers = {this.contextLessRouteProvider.Router}
                              }, new ActionDescriptor());

            var urlHelper =
                this.urlHelperFactory.GetUrlHelper(context);

            var url = urlHelper.Link(RouteNames.ProfileImageUrlRoute, new
            {
                userId = user.Id,
                id = user.ProfileImages.First().Id
            });

            return url;
        }
    }
}