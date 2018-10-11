using AutoMapper;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Presentation;
using Etdb.UserService.Services.Abstractions;

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