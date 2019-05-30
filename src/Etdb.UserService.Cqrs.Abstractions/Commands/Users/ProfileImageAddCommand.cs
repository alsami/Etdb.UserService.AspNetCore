using System;
using System.Net.Mime;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class ProfileImageAddCommand : IResponseCommand<ProfileImageMetaInfoDto>
    {
        public ProfileImageAddCommand(Guid userId, string fileName, ContentType fileContentType, byte[] fileBytes)
        {
            this.UserId = userId;
            this.FileName = fileName;
            this.FileContentType = fileContentType;
            this.FileBytes = fileBytes;
        }


        public Guid UserId { get; }

        public string FileName { get; }

        public ContentType FileContentType { get; }

        public byte[] FileBytes { get; }
    }
}