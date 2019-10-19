using System;
using System.Net.Mime;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages
{
    public class ProfileImageAddCommand : IVoidCommand
    {
        public ProfileImageAddCommand(Guid userId, string fileName, ContentType fileContentType, ReadOnlyMemory<byte> file)
        {
            this.UserId = userId;
            this.FileName = fileName;
            this.FileContentType = fileContentType;
            this.File = file;
        }


        public Guid UserId { get; }

        public string FileName { get; }

        public ContentType FileContentType { get; }

        public ReadOnlyMemory<byte> File { get; }
    }
}