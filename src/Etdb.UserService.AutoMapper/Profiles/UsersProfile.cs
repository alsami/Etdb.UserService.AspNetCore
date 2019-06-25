using AutoMapper;
using Etdb.UserService.AutoMapper.Converters;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.AutoMapper.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            this.CreateMap<User, UserDto>()
//                .ForMember(dest => dest.SignInProvider,
//                    options => options.MapFrom(src => src.AuthenticationProvider.ToString()))
//                .ForMember(dest => dest.IsExternalUser,
//                    options => options.MapFrom(src =>
//                        src.AuthenticationProvider != AuthenticationProvider.UsernamePassword))
//                .ForMember(dest => dest.EmailMentaInfoContainer, options => options.Ignore());
                .ConvertUsing<UserDtoTypeConverter>();

            this.CreateMap<UserRegisterDto, UserRegisterCommand>()
                .ConvertUsing<UserRegisterCommandTypeConverter>();
        }
    }
}