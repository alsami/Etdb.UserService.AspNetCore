using System;
using System.Collections.Generic;
using Etdb.ServiceBase.EventSourcing.Abstractions.Commands;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.EventSourcing.Abstractions.Commands
{
    public abstract class UserCommand<TResponse> : TransactionCommand<TResponse> where TResponse : class
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
