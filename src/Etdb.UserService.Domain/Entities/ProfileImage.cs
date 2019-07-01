using System;
using System.IO;
using Etdb.UserService.Domain.Base;
using Newtonsoft.Json;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.Entities
{
    public class ProfileImage : UserChildDocument
    {
        [JsonConstructor]
        private ProfileImage(Guid id, Guid userId, string name, string originalName, string mediaType,
            bool isPrimary) : base(id, userId)
        {
            this.Name = name;
            this.OriginalName = originalName;
            this.MediaType = mediaType;
            this.IsPrimary = isPrimary;
        }

        public string Name { get; private set; }

        public string OriginalName { get; private set; }

        public string MediaType { get; private set; }

        public bool IsPrimary { get; private set; }

        public string SubPath() => this.UserId.ToString();

        public string RelativePath() => Path.Combine(this.SubPath(), this.Name);

        public ProfileImage MutatePrimaryState(bool primary) => new ProfileImage(this.Id, this.UserId, this.Name,
            this.OriginalName, this.MediaType, primary);

        public static ProfileImage Create(Guid id, Guid userId, string originalName, string mediaType, bool isPrimary)
            => new ProfileImage(id, userId, $"{userId}_{originalName}", originalName, mediaType, isPrimary);
    }
}