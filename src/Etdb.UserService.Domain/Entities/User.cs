using System;
using System.Collections.Generic;
using Etdb.ServiceBase.Domain.Abstractions.Base;

namespace Etdb.UserService.Domain.Entities
{
    public class User : Entity
    {
        public User()
        {
            this.UserSecurityroles = new List<UserSecurityrole>();
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

        public ICollection<UserSecurityrole> UserSecurityroles
        {
            get;
            set;
        }
    }
}
