using System;
using Etdb.UserService.Domain.Base;
using Newtonsoft.Json;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.Entities
{
    public class ProfileImage : GuidDocument
    {
        [JsonConstructor]
        private ProfileImage(Guid id, string name, string originalName, string mediaType, bool isPrimary) : base(id)
        {
            this.Name = name;
            this.OriginalName = originalName;
            this.MediaType = mediaType;
            this.IsPrimary = isPrimary;
        }

        public string Name { get; private set; }

        public string OriginalName { get; private set; }

        public string MediaType { get; private set; }
        public bool IsPrimary { get; }

        public static ProfileImage Create(Guid id, string originalName, string mediaType, bool isPrimary)
            => new ProfileImage(id,
                $"{id}_{DateTime.UtcNow.Ticks}_{originalName}",
                originalName, mediaType, isPrimary);
    }
}