using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages
{
    public class ProfileImageSetPrimaryCommand : IVoidCommand
    {
        public Guid Id { get; }

        public Guid UserId { get; }

        public ProfileImageSetPrimaryCommand(Guid id, Guid userId)
        {
            this.Id = id;
            this.UserId = userId;
        }
    }
}