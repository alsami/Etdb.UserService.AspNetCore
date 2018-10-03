using System.Collections.Generic;

namespace Etdb.UserService.Presentation
{
    public class UserRegisterDto
    {
        public string FirstName { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public ICollection<EmailDto> Emails { get; set; }
    }
}