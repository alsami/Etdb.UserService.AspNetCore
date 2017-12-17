using AutoMapper;
using Etdb.UserService.Application.Automapper.Converter;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.Application.Automapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, UserDTO>()
                .ForMember(destination => destination.ConccurencyToken,
                    options => options.MapFrom(src => src.RowVersion))
                .ReverseMap();

            this.CreateMap<UserRegisterCommand, User>();

            this.CreateMap<UserRegisterDTO, UserRegisterCommand>()
                .ConvertUsing<UserRegisterCommandConverter>();

            this.CreateMap<UserUpdateCommand, User>()
                .ForMember(destination => destination.UserSecurityroles, options => options.Ignore());

            this.CreateMap<UserDTO, UserUpdateCommand>()
                .ConvertUsing<UserUpdateCommandConverter>();
        }
    }
}
