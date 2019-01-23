using System;
using System.Collections.Generic;
using Etdb.UserService.Presentation.Base;
// ReSharper disable UnusedMember.Global

namespace Etdb.UserService.Presentation
{
    public class UserDto : GuidDto
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Name { get; set; }

        public string Biography { get; set; }

        public DateTime RegisteredSince { get; set; }

        public string ProfileImageUrl { get; set; }

        public string SignInProvider { get; set; }

        public bool IsExternalUser { get; set; }

        public ICollection<EmailDto> Emails { get; set; }
    }
}