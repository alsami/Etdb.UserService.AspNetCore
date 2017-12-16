using Etdb.UserService.Domain.Entities;
using ETDB.API.ServiceBase.Repositories.Abstractions.Generics;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface ISecurityRoleRepository : IEntityRepository<Securityrole>
    {
        Securityrole Find(string roleName);
    }
}
