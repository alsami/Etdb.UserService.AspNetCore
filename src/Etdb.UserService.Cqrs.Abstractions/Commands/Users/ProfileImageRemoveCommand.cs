using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class ProfileImageRemoveCommand : IVoidCommand
    {
        public Guid UserId { get; }
        public Guid Id { get; }

        public ProfileImageRemoveCommand(Guid userId, Guid id)
        {
            this.UserId = userId;
            this.Id = id;
        }
    }
}