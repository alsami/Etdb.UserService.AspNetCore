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
                .ConvertUsing<UserDtoTypeConverter>();

            this.CreateMap<User, UserFlatDto>()
                .ConvertUsing<UserFlatDtoTypeConverter>();

            this.CreateMap<UserRegisterDto, UserRegisterCommand>()
                .ConvertUsing<UserRegisterCommandTypeConverter>();
        }
    }
}