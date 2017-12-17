using System;
using System.Collections.Generic;
using System.Text;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Abstractions.Commands;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.EventSourcing.Commands
{
    public class UserRegisterCommand : UserCommand<UserDTO>
    {
        public UserRegisterCommand(string name, string lastName, string email, string userName, string password, List<UserSecurityrole> userSecurityroles = null)
        {
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
            this.Password = password;
            this.UserSecurityroles = userSecurityroles ?? new List<UserSecurityrole>();
        }
    }
}
