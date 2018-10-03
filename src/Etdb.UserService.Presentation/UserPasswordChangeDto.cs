using System;
using System.Collections.Generic;
using System.Text;

namespace Etdb.UserService.Presentation
{
    public class UserPasswordChangeDto
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}