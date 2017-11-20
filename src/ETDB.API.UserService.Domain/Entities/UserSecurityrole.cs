using System;
using ETDB.API.ServiceBase.Abstractions.Entities;

namespace ETDB.API.UserService.Domain.Entities
{
    public class UserSecurityrole : IEntity
    {
        public Guid Id
        {
            get;
            set;
        }

        public byte[] RowVersion
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }

        public Guid UserId
        {
            get;
            set;
        }

        public Securityrole Securityrole
        {
            get;
            set;
        }

        public Guid SecurityroleId
        {
            get;
            set;
        }
    }
}
