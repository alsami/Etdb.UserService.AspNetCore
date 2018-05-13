using System;
using AutoMapper;
using Etdb.UserService.Constants;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Presentation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Etdb.UserService.AutoMapper.Resolver
{
    public class UserProfileImageUrlResolver : IValueResolver<User, UserDto, string>
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;

        public UserProfileImageUrlResolver(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
        }
        
        public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
        {
            if (source.ProfileImage == null)
            {
                return null;
            }
            
            var urlHelper = this.urlHelperFactory.GetUrlHelper(this.actionContextAccessor.ActionContext);

            var imageUrl = urlHelper.Link(RouteNames.UserProfileImageUrlRoute, new
            {
                source.Id,
                date = DateTime.UtcNow.Ticks
            });

            return imageUrl;
        }
    }
}