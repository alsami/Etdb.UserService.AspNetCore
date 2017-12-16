using System;
using System.Collections.Generic;
using ETDB.API.ServiceBase.Domain.Abstractions.Base;

namespace Etdb.UserService.Domain.Entities
{
    public class Securityrole : Entity
    {
        public Securityrole(Guid id, string designation, string description, bool isSystem)
        {
            this.Id = id;
            this.Designation = designation;
            this.Description = description;
            this.IsSystem = isSystem;
            this.UserSecurityroles = new List<UserSecurityrole>();
        }

        public Securityrole()
        {
            this.UserSecurityroles = new List<UserSecurityrole>();
        }

        public string Designation
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public bool IsSystem
        {
            get;
            private set;
        }

        public ICollection<UserSecurityrole> UserSecurityroles
        {
            get;
            set;
        }
    }
}
