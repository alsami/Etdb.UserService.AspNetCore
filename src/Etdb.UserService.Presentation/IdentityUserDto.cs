using System;

namespace Etdb.UserService.Presentation
{
    public class IdentityUserDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string[] Emails { get; set; }

        public string[] Roles { get; set; }

        public string AuthenticationProvider { get; set; }

        public string ProfileImageUrl { get; set; }
    }
}