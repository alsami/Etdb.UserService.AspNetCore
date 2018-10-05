using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Etdb.UserService.Constants;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Services.Abstractions;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class UserProfileImageUrlFactory : IUserProfileImageUrlFactory
    {
        // very hacky shit https://github.com/aspnet/Mvc/issues/5164
        public static IRouter Router;

        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserProfileImageUrlFactory(IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GenerateUrl(Guid userId, UserProfileImage profileImage)
        {
            if (profileImage == null)
            {
                return null;
            }

            var context = this.actionContextAccessor.ActionContext ??
                          new ActionContext(this.httpContextAccessor.HttpContext,
                              new RouteData(), new ActionDescriptor());

            if (!context.RouteData.Routers.Any())
            {
                context.RouteData.Routers.Add(Router);
            }

            var urlHelper =
                this.urlHelperFactory.GetUrlHelper(context);

            var url = urlHelper.Link(RouteNames.UserProfileImageUrlRoute, new
            {
                id = userId,
                date = DateTime.UtcNow.Ticks
            });

            return url;
        }
    }
}