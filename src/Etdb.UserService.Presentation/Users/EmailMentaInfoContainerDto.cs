using System.Collections.Generic;

namespace Etdb.UserService.Presentation.Users
{
    public class EmailMentaInfoContainerDto
    {
        public string EmailsUrl { get; set; } = null!;

        public ICollection<EmailMetaInfoDto> EmailMetaInfo { get; set; } = null!;

        public EmailMentaInfoContainerDto(string emailsUrl, ICollection<EmailMetaInfoDto> emailMetaInfo)
        {
            this.EmailsUrl = emailsUrl;
            this.EmailMetaInfo = emailMetaInfo;
        }

        public EmailMentaInfoContainerDto()
        {
        }
    }
}