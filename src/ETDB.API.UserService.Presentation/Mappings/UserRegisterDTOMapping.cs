using AutoMapper;
using ETDB.API.UserService.EventSourcing.Commands;
using ETDB.API.UserService.Presentation.DTO;

namespace ETDB.API.UserService.Presentation.Mappings
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
