using System;
using System.Net.Mime;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserProfileImageAddCommand : IResponseCommand<UserDto>
    {
        public UserProfileImageAddCommand(Guid id, string fileName, ContentType fileContentType, byte[] fileBytes)
        {
            this.Id = id;
            this.FileName = fileName;
            this.FileContentType = fileContentType;
            this.FileBytes = fileBytes;
        }

        public Guid Id { get; }

        public string FileName { get; set; }
        
        public ContentType FileContentType { get; }   

        public byte[] FileBytes { get; }
    }
}