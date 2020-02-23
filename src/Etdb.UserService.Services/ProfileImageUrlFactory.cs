using System;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.ValueObjects;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class ProfileImageUrlFactory : IProfileImageUrlFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public ProfileImageUrlFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }
        
        public string GenerateUrl(ProfileImage profileImage, Guid userId)
        {
            var url = this.linkGenerator.GetUriByName(this.httpContextAccessor.HttpContext, RouteNames.ProfileImages.LoadRoute, new
            {
                userId,
                id = profileImage.Id,
            });

            return url;
        }
        
        public string GetResizeUrl(ProfileImage profileImage, Guid userId)
        {
            var url = this.linkGenerator.GetUriByName(this.httpContextAccessor.HttpContext, RouteNames.ProfileImages.LoadResizedRoute, new
            {
                userId,
                id = profileImage.Id,
            });

            return url;
        }
        
        public string GetDeleteUrl(ProfileImage profileImage, Guid userId)
        {
            var url = this.linkGenerator.GetUriByName(this.httpContextAccessor.HttpContext, RouteNames.ProfileImages.DeleteRoute, new
            {
                userId,
                id = profileImage.Id,
            });

            return url;
        }
    }
}