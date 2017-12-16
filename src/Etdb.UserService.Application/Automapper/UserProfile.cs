using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.Application.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, UserDTO>()
                .ForMember(destination => destination.ConccurencyToken,
                    options => options.MapFrom(src => src.RowVersion))
                .ReverseMap();

            this.CreateMap<UserRegisterDTO, UserRegisterCommand>()
                .ConstructUsing(userRegister => new UserRegisterCommand(userRegister.Name,
                    userRegister.LastName, userRegister.Email, userRegister.UserName, userRegister.Password));
        }
    }
}
