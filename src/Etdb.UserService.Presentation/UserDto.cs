using System.Collections.Generic;
using Etdb.UserService.Presentation.Base;

namespace Etdb.UserService.Presentation
{
    public class UserDto : GuidDto
    {
        public string UserName { get; set; }

        public ICollection<EmailDto> Emails { get; set; }
    }
}