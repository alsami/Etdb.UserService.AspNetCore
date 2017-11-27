using ETDB.API.ServiceBase.Repositories.Abstractions.Base;
using ETDB.API.ServiceBase.Repositories.Generics;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Repositories.Abstractions;

namespace ETDB.API.UserService.Repositories
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
