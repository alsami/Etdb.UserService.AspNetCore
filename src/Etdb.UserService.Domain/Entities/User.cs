using System;
using System.Collections.Generic;
using Etdb.ServiceBase.Domain.Abstractions.Base;

namespace Etdb.UserService.Domain.Entities
{
    public class User : Entity
    {
        public User(Guid id, string name, string lastName, string email, string userName, string password, byte[] salt)
        {
            this.Id = id;
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
            this.Password = password;
            this.Salt = salt;
            this.UserSecurityroles = new List<UserSecurityrole>();
        }

        public User()
        {
            this.UserSecurityroles = new List<UserSecurityrole>();
        }

        public string Name
        {
            get;
            private set;
        }

        public string LastName
        {
            get;
            private set;
        }

        public string UserName
        {
            get;
            private set;
        }

        public string Email
        {
            get;
            private set;
        }

        public byte[] Salt
        {
            get;
            private set;
        }

        public string Password
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
