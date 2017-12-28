using System.Collections.Generic;
using Etdb.UserService.EventSourcing.Abstractions.Commands;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.EventSourcing.Commands
{
    public class UserRegisterCommand : UserCommand<UserDTO>
    {
        public UserRegisterCommand(string name, string lastName, string email, string userName, string password)
        {
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
            this.Password = password;
        }
    }
}
