using System;

// ReSharper disable MemberCanBePrivate.Global

namespace Etdb.UserService.Presentation.Users
{
    public class ProfileImageMetaInfoDto
    {
        public Guid Id { get; set; }

        public string Url { get; set; } = null!;

        public string ResizeUrl { get; set; } = null!;

        public string RemoveUrl { get; set; } = null!;

        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }

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

        public ProfileImageMetaInfoDto()
        {
        }
    }
}