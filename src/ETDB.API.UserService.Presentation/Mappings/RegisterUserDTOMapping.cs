using AutoMapper;
using ETDB.API.ServiceBase.Hasher;
using ETDB.API.UserService.EventSourcing.Commands;
using ETDB.API.UserService.Presentation.DTO;

namespace ETDB.API.UserService.Presentation.Mappings
{
    public class RegisterUserDTOMapping : Profile
    {
        public RegisterUserDTOMapping()
        {
            var hasher = new Hasher();
            var salt = hasher.GenerateSalt();
            this.CreateMap<RegisterUserDTO, UserRegisterCommand>()
                .ConstructUsing(registerUser => new UserRegisterCommand(registerUser.Name,
                    registerUser.LastName, registerUser.Email, registerUser.UserName,
                    hasher.CreateSaltedHash(registerUser.Password, salt), salt));
        }
    }
}
