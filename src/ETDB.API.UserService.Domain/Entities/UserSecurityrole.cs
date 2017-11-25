using System;
using ETDB.API.ServiceBase.Domain.Abstractions.Base;

namespace ETDB.API.UserService.Domain.Entities
{
    public class UserSecurityrole : Entity
    {
        public UserSecurityrole(Guid id, Guid userId, Guid securityroleId)
        {
            this.Id = id;
            this.UserId = userId;
            this.SecurityroleId = securityroleId;
        }

        public UserSecurityrole()
        {
            
        }

        public User User
        {
            get;
            set;
        }

        public Guid UserId
        {
            get;
            private set;
        }

        public Securityrole Securityrole
        {
            get;
            set;
        }

        public Guid SecurityroleId
        {
            get;
            private set;
        }
    }
}
