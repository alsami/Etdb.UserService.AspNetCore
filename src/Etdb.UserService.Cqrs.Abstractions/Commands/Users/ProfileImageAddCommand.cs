using System;
using System.Net.Mime;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class ProfileImageAddCommand : IResponseCommand<UserDto>
    {
        public ProfileImageAddCommand(Guid userId, string fileName, ContentType fileContentType, byte[] fileBytes, bool isPrimary)
        {
            this.UserId = userId;
            this.FileName = fileName;
            this.FileContentType = fileContentType;
            this.FileBytes = fileBytes;
            this.IsPrimary = isPrimary;
        }

        public ProfileImageAddCommand(string fileName, ContentType fileContentType, byte[] fileBytes, bool isPrimary)
        {
            this.FileName = fileName;
            this.FileContentType = fileContentType;
            this.FileBytes = fileBytes;
            this.IsPrimary = isPrimary;
        }

        public Guid UserId { get; } = Guid.Empty;

        public string FileName { get; }

        public ContentType FileContentType { get; }
        
        public bool IsPrimary { get; }

        public byte[] FileBytes { get; }
    }
}