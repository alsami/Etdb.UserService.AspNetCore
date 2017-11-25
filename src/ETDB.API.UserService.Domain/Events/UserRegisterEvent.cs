using System;
using System.Collections.Generic;
using System.Text;
using ETDB.API.ServiceBase.Domain.Abstractions.Events;
using ETDB.API.UserService.Domain.Entities;

namespace ETDB.API.UserService.Domain.Events
{
    public class UserRegisterEvent : Event
    {
        public UserRegisterEvent(Guid id, string name, string lastName, string email, 
            string userName, string password, 
            byte[] salt, byte[] rowVersion, ICollection<UserSecurityrole> userSecurityroles)
        {
            this.Id = id;
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
            this.Password = password;
            this.Salt = salt;
            this.UserSecurityroles = userSecurityroles;
            this.RowVersion = rowVersion;
            this.AggregateId = id;
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
        }

        public string Email
        {
            get;
        }

        public byte[] Salt
        {
            get;
        }

        public string Password
        {
            get;
        }

        public ICollection<UserSecurityrole> UserSecurityroles
        {
            get;
            set;
        }
    }
}
