using System.Threading.Tasks;
using Etdb.ServiceBase.Repositories.Abstractions.Generics;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface ISecurityRoleRepository : IEntityRepository<SecurityRole>
    {
        Task<SecurityRole> FindAsync(string roleName);
    }
}
