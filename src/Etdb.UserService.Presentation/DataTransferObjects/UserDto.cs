using System.Collections.Generic;
using Etdb.UserService.Presentation.DataTransferObjects.Base;

namespace Etdb.UserService.Presentation.DataTransferObjects
{
    public class UserDto : TrackedDto
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public ICollection<SecurityRoleDto> SecurityRoles { get; set; }
    }
}
