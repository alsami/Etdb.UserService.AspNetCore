using AutoMapper;
using Etdb.UserService.Application.Automapper.Converter;
using Etdb.UserService.Application.Automapper.Resolver;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.EventSourcing.Events;
using Etdb.UserService.Presentation.DataTransferObjects;

namespace Etdb.UserService.Application.Automapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, UserDto>()
                .ForMember(destination => destination.ConccurencyToken,
                    options => options.MapFrom(src => src.RowVersion))
                .ForMember(destination => destination.SecurityRoles, options => options.ResolveUsing<SecurityRolesResolver>())
                .ReverseMap();

            this.CreateMap<User, UserRegisterEvent>()
                .ConvertUsing<UserRegisterEventConverter>();

            this.CreateMap<User, UserUpdateEvent>()
                .ConvertUsing<UserUpdateEventConverter>();

            this.CreateMap<UserRegisterCommand, User>();

            this.CreateMap<UserRegisterDto, UserRegisterCommand>()
                .ConvertUsing<UserRegisterCommandConverter>();

            this.CreateMap<UserUpdateCommand, User>();

            this.CreateMap<UserDto, UserUpdateCommand>()
                .ConvertUsing<UserUpdateCommandConverter>();
        }
    }
}
