using System;
using System.Collections.Generic;
using System.Text;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Abstractions.Commands;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.EventSourcing.Commands
{
    public class UserUpdateCommand : UserCommand<UserDTO>
    {
        public UserUpdateCommand(Guid id, byte[] rowVersion, string name, string lastName, string email, string userName, List<UserSecurityrole> userSecurityroles = null)
        {
            this.Id = id;
            this.RowVersion = rowVersion;
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
            this.UserSecurityroles = userSecurityroles ?? new List<UserSecurityrole>();
        }
    }
}
