using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.Application.Mappings
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
