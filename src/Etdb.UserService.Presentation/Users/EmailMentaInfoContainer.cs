using System.Collections.Generic;

namespace Etdb.UserService.Presentation.Users
{
    public class EmailMentaInfoContainer
    {
        public string EmailsUrl { get; }

        public ICollection<EmailMetaInfoDto> EmailMetaInfo { get; }

        public EmailMentaInfoContainer(string emailsUrl, ICollection<EmailMetaInfoDto> emailMetaInfo)
        {
            this.EmailsUrl = emailsUrl;
            this.EmailMetaInfo = emailMetaInfo;
        }
    }
}