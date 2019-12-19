using System;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages
{
    public class ProfileImageRemoveCommand : IRequest
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