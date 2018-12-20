using System;
using Etdb.UserService.Domain.Base;
using Newtonsoft.Json;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.Entities
{
    public class UserProfileImage : GuidDocument
    {
        [JsonConstructor]
        private UserProfileImage(Guid id, string name, string originalName, string mediaType) : base(id)
        {
            this.Name = name;
            this.OriginalName = originalName;
            this.MediaType = mediaType;
        }

        public string Name { get; private set; }

        public string OriginalName { get; private set; }

        public string MediaType { get; private set; }

        public static UserProfileImage Create(Guid id, string originalName, string mediaType)
            => new UserProfileImage(id,
                $"{id}_{DateTime.UtcNow.Ticks}_{originalName}",
                originalName, mediaType);
    }
}