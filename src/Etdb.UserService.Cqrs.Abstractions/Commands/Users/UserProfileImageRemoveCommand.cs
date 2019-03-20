using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserProfileImageRemoveCommand : IVoidCommand
    {
        public Guid Id { get; }

        public UserProfileImageRemoveCommand(Guid id)
        {
            this.Id = id;
        }
    }
}