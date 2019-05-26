using System;
using System.IO;
using Etdb.UserService.Domain.Base;
using Newtonsoft.Json;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.Entities
{
    public class ProfileImage : GuidDocument
    {
        [JsonConstructor]
        private ProfileImage(Guid id, Guid userId, string name, string originalName, string mediaType,
            bool isPrimary) : base(id)
        {
            this.UserId = userId;
            this.Name = name;
            this.OriginalName = originalName;
            this.MediaType = mediaType;
            this.IsPrimary = isPrimary;
        }

        public Guid UserId { get; private set; }

        public string Name { get; private set; }

        public string OriginalName { get; private set; }

        public string MediaType { get; private set; }

        public bool IsPrimary { get; private set; }

        public string Subpath() => this.UserId.ToString();

        public string RelativePath() => Path.Combine(this.Subpath(), this.Name);

        public static ProfileImage Create(Guid id, Guid userId, string originalName, string mediaType, bool isPrimary)
            => new ProfileImage(id, userId, $"{userId}_{originalName}", originalName, mediaType, isPrimary);
    }
}