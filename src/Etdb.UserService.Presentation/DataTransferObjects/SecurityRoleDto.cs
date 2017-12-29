using System.Collections.Generic;
using Etdb.UserService.Presentation.DataTransferObjects.Base;

namespace Etdb.UserService.Presentation.DataTransferObjects
{
    public class SecurityRoleDto : TrackedDto
    {
        public string Designation { get; set; }
        public string Description { get; set; }
    }
}
