using System;
using System.Collections.Generic;
using Etdb.ServiceBase.Repositories.Abstractions.Base;
using Etdb.ServiceBase.Repositories.Generics;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories
{
    public class UserSecurityRoleRepository : EntityRepository<UserSecurityrole>, IUserSecurityRoleRepository
    {
        public UserSecurityRoleRepository(AppContextBase context) : base(context)
        {
        }

        public UserSecurityrole Find(string roleName, bool includeUser = false)
        {
            throw new NotImplementedException();
        }

        IEnumerable<UserSecurityrole> IUserSecurityRoleRepository.Find(string roleName, string userName, bool includeUser)
        {
            throw new NotImplementedException();
        }
    }
}
