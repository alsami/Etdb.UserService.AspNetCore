using AutoMapper;
using Etdb.UserService.AutoMapper.Resolver;
using Etdb.UserService.AutoMapper.TypeConverters;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.AutoMapper.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            this.CreateMap<User, UserDto>()
                .ForMember(destination => destination.ProfileImageUrl,
                    options => options.ResolveUsing<UserProfileImageUrlResolver>());

            this.CreateMap<UserRegisterDto, UserRegisterCommand>()
                .ConvertUsing<UserRegisterCommandTypeConverter>();

            this.CreateMap<Email, EmailDto>();
        }
    }
}