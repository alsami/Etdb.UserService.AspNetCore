using AutoMapper;
using ETDB.API.UserService.Domain.Entities;

namespace ETDB.API.UserService.Presentation.DataTransferObjects.Mappings
{
    public class RegisterUserDTOMapping : Profile
    {
        public RegisterUserDTOMapping()
        {
            this.CreateMap<RegisterUserDTO, User>();
        }
    }
}
