using System;
using System.Collections.Generic;
using ETDB.API.ServiceBase.Abstractions.Entities;

namespace ETDB.API.UserService.Domain.Entities
{
    public class Securityrole : IEntity
    {
        public Securityrole()
        {
            this.UserSecurityroles = new List<UserSecurityrole>();
        }

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

        public string Designation
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public ICollection<UserSecurityrole> UserSecurityroles
        {
            get;
            set;
        }
    }
}
