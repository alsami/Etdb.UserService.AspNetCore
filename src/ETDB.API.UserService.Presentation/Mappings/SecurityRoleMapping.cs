using AutoMapper;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Presentation.DTO;

namespace ETDB.API.UserService.Presentation.Mappings
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
