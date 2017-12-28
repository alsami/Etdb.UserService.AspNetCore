using System.Threading.Tasks;
using Etdb.ServiceBase.Repositories.Abstractions.Base;
using Etdb.ServiceBase.Repositories.Generics;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories
{
    public class SecurityRoleRepository : EntityRepository<SecurityRole>, ISecurityRoleRepository
    {
        public SecurityRoleRepository(AppContextBase context) : base(context)
        {
        }

        public async Task<SecurityRole> FindAsync(string roleName)
        {
            var securityRole = await this.GetAsync(role => role.Designation == roleName);

            return securityRole;
        }
    }
}
