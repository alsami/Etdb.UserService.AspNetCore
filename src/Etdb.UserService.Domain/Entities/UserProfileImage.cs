using System;
using Etdb.UserService.Domain.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.Entities
{
    public class UserProfileImage : GuidDocument
    {
        public UserProfileImage(Guid id, string name, string originalName, string mediaType) : base(id)
        {
            this.Name = name;
            this.OriginalName = originalName;
            this.MediaType = mediaType;
        }

        public string Name { get; private set; }

        public string OriginalName { get; private set; }

        public string MediaType { get; private set; }
    }
}