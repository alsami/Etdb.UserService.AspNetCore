using AutoMapper;
using Etdb.UserService.AutoMapper.Resolver;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.AutoMapper.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            this.CreateMap<User, UserDto>()
                .ForMember(destination => destination.ProfileImageUrl, options => options.ResolveUsing<UserProfileImageUrlResolver>());

            this.CreateMap<Email, EmailDto>();
        }
    }
}