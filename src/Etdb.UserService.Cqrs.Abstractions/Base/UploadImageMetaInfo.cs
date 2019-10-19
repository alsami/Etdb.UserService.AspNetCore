using System;
using System.Net.Mime;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public sealed class UploadImageMetaInfo
    {
        public string Name { get; }

        public ContentType ContentType { get; }

        public ReadOnlyMemory<byte> Image { get; }

        public UploadImageMetaInfo(string name, ContentType contentType, ReadOnlyMemory<byte> image)
        {
            this.Name = name;
            this.ContentType = contentType;
            this.Image = image;
        }
    }
}