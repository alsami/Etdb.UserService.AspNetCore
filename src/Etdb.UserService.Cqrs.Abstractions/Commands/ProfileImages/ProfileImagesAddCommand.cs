using System;
using System.Collections.Generic;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages
{
    public class ProfileImagesAddCommand : IResponseCommand<IEnumerable<ProfileImageMetaInfoDto>>
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