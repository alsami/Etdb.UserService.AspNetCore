using System;

namespace Etdb.UserService.Presentation.Users
{
    public class EmailMetaInfoDto
    {
        public Guid Id { get; }

        public string Url { get; }

        public string RemoveUrl { get; }

        public bool IsPrimary { get; }

        public bool IsExternal { get; }

        public EmailMetaInfoDto(Guid id, string url, string removeUrl, bool isPrimary, bool isExternal)
        {
            this.Id = id;
            this.Url = url;
            this.RemoveUrl = removeUrl;
            this.IsPrimary = isPrimary;
            this.IsExternal = isExternal;
        }
    }
}