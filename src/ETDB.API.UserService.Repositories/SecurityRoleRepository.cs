using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;
using ETDB.API.ServiceBase.Repositories.Abstractions.Base;
using ETDB.API.ServiceBase.Repositories.Generics;

namespace Etdb.UserService.Repositories
{
    public class SecurityRoleRepository : EntityRepository<Securityrole>, ISecurityRoleRepository
    {
        public SecurityRoleRepository(AppContextBase context) : base(context)
        {
        }

        public Securityrole Find(string roleName)
        {
            var securityRole = this.Get(role => role.Designation == roleName);

            return securityRole;
        }
    }
}
