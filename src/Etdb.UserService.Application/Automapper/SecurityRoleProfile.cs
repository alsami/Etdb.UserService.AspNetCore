using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.Application.Automapper
{
    public class SecurityRoleProfile : Profile
    {
        public SecurityRoleProfile()
        {
            this.CreateMap<Securityrole, SecurityRoleDTO>()
                .ReverseMap();
        }
    }
}
