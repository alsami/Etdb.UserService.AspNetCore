using System;
using System.Collections.Generic;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Presentation.Users;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages
{
    public class ProfileImagesAddCommand : IRequest<IEnumerable<ProfileImageMetaInfoDto>>
    {
        public Guid UserId { get; }

        public ICollection<UploadImageMetaInfo> UploadImageMetaInfos { get; }

        public ProfileImagesAddCommand(Guid userId, ICollection<UploadImageMetaInfo> uploadImageMetaInfos)
        {
            this.UserId = userId;
            this.UploadImageMetaInfos = uploadImageMetaInfos;
        }
    }
}