using System;
using System.Collections.Generic;
using Etdb.UserService.Presentation.DataTransferObjects.Base;

namespace Etdb.UserService.Presentation.DataTransferObjects
{
    public class UserDto : DataTransferObject
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public ICollection<SecurityRoleInfoDto> SecurityRoles { get; set; }
    }
}
