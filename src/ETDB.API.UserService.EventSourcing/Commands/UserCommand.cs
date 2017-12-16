using System;
using System.Collections.Generic;
using Etdb.UserService.Domain.Entities;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Commands;

namespace Etdb.UserService.EventSourcing.Commands
{
    public abstract class UserCommand : SourcingCommand
    {
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
