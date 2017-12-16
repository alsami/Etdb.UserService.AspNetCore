using AutoMapper;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.Application.Mappings
{
    public class UserRegisterDTOMapping : Profile
    {
        public UserRegisterDTOMapping()
        {
            this.CreateMap<UserRegisterDTO, UserRegisterCommand>()
                .ConstructUsing(userRegister => new UserRegisterCommand(userRegister.Name,
                    userRegister.LastName, userRegister.Email, userRegister.UserName, userRegister.Password));
        }
    }
}
