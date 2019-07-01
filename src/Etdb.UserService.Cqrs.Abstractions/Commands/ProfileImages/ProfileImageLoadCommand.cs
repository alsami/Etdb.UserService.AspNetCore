using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages
{
    public class ProfileImageLoadCommand : IResponseCommand<FileDownloadInfoDto>
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