using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class ProfileImageRemoveCommand : IVoidCommand
    {
        public Guid UserId { get; }

        public ProfileImageRemoveCommand(Guid userId)
        {
            this.UserId = userId;
        }
    }
}