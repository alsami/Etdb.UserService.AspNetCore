using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.Application.Mappings
{
    public class SecurityRoleMapping : Profile
    {
        public SecurityRoleMapping()
        {
            this.CreateMap<Securityrole, SecurityRoleDTO>()
                .ReverseMap();
        }
    }
}
