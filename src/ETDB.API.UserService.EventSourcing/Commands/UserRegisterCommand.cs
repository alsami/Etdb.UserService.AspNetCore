using System.Collections.Generic;
using ETDB.API.UserService.Domain.Entities;

namespace ETDB.API.UserService.EventSourcing.Commands
{
    public class UserRegisterCommand : UserCommand
    {
        public UserRegisterCommand(string name, string lastName, string email, string userName, string password)
        {
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
            this.Password = password;
            this.UserSecurityroles = new List<UserSecurityrole>();
        }
    }
}
