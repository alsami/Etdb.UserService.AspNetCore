﻿using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class UserNameCommand : IVoidCommand
    {
        protected UserNameCommand(string userName)
        {
            this.UserName = userName;
        }

        protected UserNameCommand(Guid id, string userName)
        {
            this.Id = id;
            this.UserName = userName;
        }

        public Guid Id { get; }

        public string UserName { get; }
    }
}