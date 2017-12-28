using System;
using System.Collections.Generic;
using Etdb.UserService.EventSourcing.Abstractions.Commands;
using Etdb.UserService.Presentation.DataTransferObjects;

namespace Etdb.UserService.EventSourcing.Commands
{
    public class UserUpdateCommand : UserCommand<UserDto>
    {
        public UserUpdateCommand(Guid id, byte[] rowVersion, string name, string lastName, string email, string userName)
        {
            this.Id = id;
            this.RowVersion = rowVersion;
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
        }
    }
}
