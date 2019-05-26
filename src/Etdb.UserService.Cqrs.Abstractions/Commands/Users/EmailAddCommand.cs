using System;
using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class EmailAddCommand : EmailCommand
    {
        public EmailAddCommand(Guid id, string address, bool isPrimary, bool isExternal) : base(id, address, isPrimary,
            isExternal)
        {
        }
    }
}