using System;
using Etdb.UserService.Constants;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class UserProfileImageUrlFactory : IUserProfileImageUrlFactory
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ContextLessRouteProvider contextLessRouteProvider;

        public UserProfileImageUrlFactory(IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor,
            ContextLessRouteProvider contextLessRouteProvider)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
            this.httpContextAccessor = httpContextAccessor;
            this.contextLessRouteProvider = contextLessRouteProvider;
        }

        public string GenerateUrl(Guid userId, UserProfileImage profileImage)
        {
            if (profileImage == null)
            {
                return null;
            }

            var context = this.actionContextAccessor.ActionContext ??
                          new ActionContext(this.httpContextAccessor.HttpContext,
                              new RouteData
                              {
                                  Routers = {this.contextLessRouteProvider.Router}
                              }, new ActionDescriptor());

            var urlHelper =
                this.urlHelperFactory.GetUrlHelper(context);

            var url = urlHelper.Link(RouteNames.UserProfileImageUrlRoute, new
            {
                id = userId,
                date = DateTime.UtcNow.Ticks
            });

            //// new {
            ////    id = userId,
            ////    date = DateTime.UtcNow.Ticks
            ////}

            //var url = linkGenerator.GetUriByAddress(this.httpContextAccessor.HttpContext, new RouteValueDictionary(new
            //{
            //    controller = ControllerNames.UsersController,
            //    action = RouteNames.UserProfileImageUrlRoute,
            //    id = userId
            //}), httpContextAccessor.HttpContext.Request.Scheme, httpContextAccessor.HttpContext.Request.Host);

            return url;
        }
    }
}