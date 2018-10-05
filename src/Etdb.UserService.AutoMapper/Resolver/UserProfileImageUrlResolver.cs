using System;
using AutoMapper;
using Etdb.UserService.Constants;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Presentation;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Etdb.UserService.AutoMapper.Resolver
{
    public class UserProfileImageUrlResolver : IValueResolver<User, UserDto, string>
    {
        private readonly IUserProfileImageUrlFactory userProfileImageUrlFactory;

        public UserProfileImageUrlResolver(IUserProfileImageUrlFactory userProfileImageUrlFactory)
        {
            this.userProfileImageUrlFactory = userProfileImageUrlFactory;
        }


        public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
        {
            return this.userProfileImageUrlFactory.GenerateUrl(source.Id, source.ProfileImage);
        }
    }
}