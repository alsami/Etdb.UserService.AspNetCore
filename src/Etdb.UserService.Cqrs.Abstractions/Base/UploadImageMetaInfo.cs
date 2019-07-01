using System.Net.Mime;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public sealed class UploadImageMetaInfo
    {
        public string Name { get; }

        public ContentType ContentType { get; }

        public byte[] Bytes { get; }

        public UploadImageMetaInfo(string name, ContentType contentType, byte[] bytes)
        {
            this.Name = name;
            this.ContentType = contentType;
            this.Bytes = bytes;
        }
    }
}