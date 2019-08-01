using System;

// ReSharper disable MemberCanBePrivate.Global

namespace Etdb.UserService.Presentation.Users
{
    public class ProfileImageMetaInfoDto
    {
        public Guid Id { get; }

        public string Url { get; }

        public string ResizeUrl { get; }

        public string RemoveUrl { get; }

        public bool IsPrimary { get; }
        public DateTime CreatedAt { get; }

        public ProfileImageMetaInfoDto(Guid id, string url, string resizeUrl, string removeUrl, bool isPrimary,
            DateTime createdAt)
        {
            this.Id = id;
            this.Url = url;
            this.ResizeUrl = resizeUrl;
            this.RemoveUrl = removeUrl;
            this.IsPrimary = isPrimary;
            this.CreatedAt = createdAt;
        }
    }
}