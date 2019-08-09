using Etdb.UserService.Domain.Base;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class UserUrlFactory : IUserUrlFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly ContextLessRouteProvider contextLessRouteProvider;
        private readonly IUrlHelperFactory urlHelperFactory;

        public UserUrlFactory(IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor,
            ContextLessRouteProvider contextLessRouteProvider, IUrlHelperFactory urlHelperFactory)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.actionContextAccessor = actionContextAccessor;
            this.contextLessRouteProvider = contextLessRouteProvider;
            this.urlHelperFactory = urlHelperFactory;
        }

        private IUrlHelper Create()
        {
            var context = this.actionContextAccessor.ActionContext ??
                          new ActionContext(this.httpContextAccessor.HttpContext,
                              new RouteData
                              {
                                  Routers = {this.contextLessRouteProvider.Router}
                              }, new ActionDescriptor());

            return this.urlHelperFactory.GetUrlHelper(context);
        }

        public string GenerateUrlWithChildIdParameter<TUserChild>(TUserChild child, string route)
            where TUserChild : UserChildDocument
        {
            var urlHelper = this.Create();

            var url = urlHelper.Link(route, new
            {
                userId = child.UserId,
                id = child.Id,
            });

            return url;
        }

        public string GenerateUrl(User user, string route)
        {
            var urlHelper = this.Create();

            var url = urlHelper.Link(route, new
            {
                userId = user.Id,
            });

            return url;
        }
    }
}