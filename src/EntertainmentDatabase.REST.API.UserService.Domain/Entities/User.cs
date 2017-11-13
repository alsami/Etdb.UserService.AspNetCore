using System;
using System.Collections.Generic;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.UserService.Domain.Entities
{
    public class User : IEntity
    {
        public User()
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

        public bool IsActive
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
