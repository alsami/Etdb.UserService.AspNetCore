using ETDB.API.ServiceBase.Repositories.Abstractions.Generics;
using ETDB.API.UserService.Domain.Entities;

namespace ETDB.API.UserService.Repositories.Abstractions
{
    public interface ISecurityRoleRepository : IEntityRepository<Securityrole>
    {
        Securityrole Find(string roleName);
    }
}
