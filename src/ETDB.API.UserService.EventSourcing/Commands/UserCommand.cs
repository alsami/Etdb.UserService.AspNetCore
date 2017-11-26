﻿using System;
using System.Collections.Generic;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Commands;
using ETDB.API.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETDB.API.UserService.EventSourcing.Commands
{
    public class UserCommand : SourcingCommand
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

        public override bool IsValid()
        {
            // TODO: Implement fluent validators
            return true;
        }
    }
}
