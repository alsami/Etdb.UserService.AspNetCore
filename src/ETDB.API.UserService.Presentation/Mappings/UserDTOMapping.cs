using AutoMapper;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Presentation.DTO;

namespace ETDB.API.UserService.Presentation.Mappings
{
    public class UserDTOMapping : Profile
    {
        public UserDTOMapping()
        {
            this.CreateMap<User, UserDTO>()
                .ReverseMap();
        }
    }
}
