using Etdb.UserService.Domain.Base;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class UserChildUrlFactory<TUserChild> :IUserChildUrlFactory<TUserChild> where TUserChild : UserChildDocument
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly ContextLessRouteProvider contextLessRouteProvider;
        private readonly IUrlHelperFactory urlHelperFactory;

        public UserChildUrlFactory(IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor, ContextLessRouteProvider contextLessRouteProvider, IUrlHelperFactory urlHelperFactory)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.actionContextAccessor = actionContextAccessor;
            this.contextLessRouteProvider = contextLessRouteProvider;
            this.urlHelperFactory = urlHelperFactory;
        }

        public string GenerateUrl(TUserChild child, string route)
        {
            var urlHelper = this.Create();
            
            var url = urlHelper.Link(route, new
            {
                userId = child.UserId,
                id = child.Id,
            });

            return url;
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
    }
}