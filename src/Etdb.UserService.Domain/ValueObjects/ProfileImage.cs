using System;
using Newtonsoft.Json;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.ValueObjects
{
    public class ProfileImage
    {
        [JsonConstructor]
        private ProfileImage(Guid id, DateTime createdAt, string name, string originalName,
            string mediaType,
            bool isPrimary)
        {
            this.Id = id;
            this.CreatedAt = createdAt;
            this.Name = name;
            this.OriginalName = originalName;
            this.MediaType = mediaType;
            this.IsPrimary = isPrimary;
        }

        public Guid Id { get; private set; }

        public DateTime CreatedAt { get; private set; }
        
        public string Name { get; private set; }

        public string OriginalName { get; private set; }

        public string MediaType { get; private set; }

        public bool IsPrimary { get; private set; }

        public ProfileImage MutatePrimaryState(bool primary)
        {
            return new ProfileImage(this.Id, this.CreatedAt,
                this.Name,
                this.OriginalName, this.MediaType, primary);
        }

        public static ProfileImage Create(Guid id, Guid userId, string originalName, string mediaType, bool isPrimary)
        {
            return new ProfileImage(id, DateTime.UtcNow, $"{userId}_{originalName}_{DateTime.UtcNow.Ticks}",
                originalName, mediaType, isPrimary);
        }
    }
}