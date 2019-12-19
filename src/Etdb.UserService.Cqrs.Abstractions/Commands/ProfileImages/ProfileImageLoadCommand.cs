using System;
using Etdb.UserService.Presentation.Users;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages
{
    public class ProfileImageLoadCommand : IRequest<FileDownloadInfoDto>
    {
        public ProfileImageLoadCommand(Guid id, Guid userId)
        {
            this.Id = id;
            this.UserId = userId;
        }

        public Guid Id { get; }

        public Guid UserId { get; }
    }
}