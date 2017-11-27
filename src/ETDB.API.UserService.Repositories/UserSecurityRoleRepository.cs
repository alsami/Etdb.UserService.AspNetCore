using System;
using System.Collections.Generic;
using ETDB.API.ServiceBase.Repositories.Abstractions.Base;
using ETDB.API.ServiceBase.Repositories.Generics;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Repositories.Abstractions;

namespace ETDB.API.UserService.Repositories
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
