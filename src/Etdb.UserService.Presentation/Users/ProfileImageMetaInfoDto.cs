using System;

namespace Etdb.UserService.Presentation.Users
{
    public class ProfileImageMetaInfoDto
    {
        public Guid Id { get; }

        public string Url { get; }

        public string RemoveUrl { get; }

        public bool IsPrimary { get; }
        public DateTime CreatedAt { get; }

        public ProfileImageMetaInfoDto(Guid id, string url, string removeUrl, bool isPrimary, DateTime createdAt)
        {
            this.Id = id;
            this.Url = url;
            this.RemoveUrl = removeUrl;
            this.IsPrimary = isPrimary;
            this.CreatedAt = createdAt;
        }
    }
}