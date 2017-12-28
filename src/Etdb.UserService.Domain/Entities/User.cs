using System.Collections.Generic;
using Etdb.ServiceBase.Domain.Abstractions.Base;
using Etdb.UserService.Domain.EntityInfos;

namespace Etdb.UserService.Domain.Entities
{
    public class User : TrackedEntity
    {
        public User()
        {
            this.SecurityRoles = new List<SecurityRoleInfo>();
        }

        public string Name
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public byte[] Salt
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public ICollection<SecurityRoleInfo> SecurityRoles { get; set; }
    }
}
