using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages
{
    public class ProfileImageResizedLoadCommand : IResponseCommand<FileDownloadInfoDto>
    {
        public ProfileImageResizedLoadCommand(Guid id, Guid userId, int dimensionX, int dimensionY)
        {
            this.Id = id;
            this.UserId = userId;
            this.DimensionX = dimensionX;
            this.DimensionY = dimensionY;
        }

        public Guid Id { get; }

        public Guid UserId { get; }
        public int DimensionX { get; }
        public int DimensionY { get; }
    }
}