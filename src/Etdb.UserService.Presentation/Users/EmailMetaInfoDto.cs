using System;

namespace Etdb.UserService.Presentation.Users
{
    public class EmailMetaInfoDto
    {
        public Guid Id { get; set; }

        public string Url { get; set; } = null!;

        public string RemoveUrl { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public bool IsExternal { get; set; }

        public EmailMetaInfoDto(Guid id, string url, string removeUrl, bool isPrimary, bool isExternal)
        {
            this.Id = id;
            this.Url = url;
            this.RemoveUrl = removeUrl;
            this.IsPrimary = isPrimary;
            this.IsExternal = isExternal;
        }

        public EmailMetaInfoDto()
        {
        }
    }
}