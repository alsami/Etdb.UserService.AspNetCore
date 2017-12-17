using System;
using System.Collections.Generic;
using System.Text;
using Etdb.ServiceBase.EventSourcing.Abstractions.Events;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.EventSourcing.Abstractions.Events
{
    public abstract class UserEvent : Event
    {
        protected UserEvent(Guid id, string name, string lastName, string email,
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
            protected set;
        }

        public byte[] RowVersion
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string LastName
        {
            get;
            protected set;
        }

        public string UserName
        {
            get;
            protected set;
        }

        public string Email
        {
            get;
            protected set;
        }

        public byte[] Salt
        {
            get;
            protected set;
        }

        public string Password
        {
            get;
            protected set;
        }

        public ICollection<UserSecurityrole> UserSecurityroles
        {
            get;
            protected set;
        }
    }
}
