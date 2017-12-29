using System.Collections.Generic;
using Etdb.UserService.EventSourcing.Abstractions.Commands;
using Etdb.UserService.Presentation.DataTransferObjects;

namespace Etdb.UserService.EventSourcing.Commands
{
    public class SecurityRoleUpdateCommand : SecurityRoleCommand<SecurityRoleDto>
    {
        //public SecurityRoleUpdateCommand(string id, string description, byte[] rowVersion, ICollection<UserInfo> users)
        //{
        //    this.Id = id;
        //    this.Description = description;
        //    this.RowVersion = rowVersion;
        //    this.Users = users;
        //}
    }
}
