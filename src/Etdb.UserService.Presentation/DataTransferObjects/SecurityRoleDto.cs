using System;
using Etdb.UserService.Presentation.DataTransferObjects.Base;

namespace Etdb.UserService.Presentation.DataTransferObjects
{
    public class SecurityRoleDto : DataTransferObject
    {
        public string Designation { get; set; }
        public string Description { get; set; }
    }
}
