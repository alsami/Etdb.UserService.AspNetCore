using System;
using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Domain.Documents
{
    public class UserProfileImage : GuidDocument
    {
        public UserProfileImage(Guid id, string name, string originalName, string mediaType) : base(id)
        {
            this.Name = name;
            this.OriginalName = originalName;
            this.MediaType = mediaType;
        }

        public string Name { get; }

        public string OriginalName { get; }

        public string MediaType { get; }
    }
}