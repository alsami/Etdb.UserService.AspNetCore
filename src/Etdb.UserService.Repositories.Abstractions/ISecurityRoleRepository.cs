using Etdb.ServiceBase.Repositories.Abstractions.Generics;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface ISecurityRoleRepository : IEntityRepository<Securityrole>
    {
        Securityrole Find(string roleName);
    }
}
