using System;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages
{
    public class ProfileImageSetPrimaryCommand : IRequest
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