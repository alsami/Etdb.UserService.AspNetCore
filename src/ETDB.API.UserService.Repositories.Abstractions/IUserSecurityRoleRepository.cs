using System.Collections.Generic;
using ETDB.API.ServiceBase.Repositories.Abstractions.Generics;
using ETDB.API.UserService.Domain.Entities;
using Remotion.Linq.Clauses.ExpressionVisitors;

namespace ETDB.API.UserService.Repositories.Abstractions
{
    public interface IUserSecurityRoleRepository : IEntityRepository<UserSecurityrole>
    {
        UserSecurityrole Find(string roleName, bool includeUser = false);

        IEnumerable<UserSecurityrole> Find(string roleName, string userName, bool includeUser = false);
    }
}
