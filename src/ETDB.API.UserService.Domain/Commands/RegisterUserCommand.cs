using System;
using System.Collections.Generic;
using System.Text;
using ETDB.API.UserService.Domain.Entities;

namespace ETDB.API.UserService.Domain.Commands
{
    public class RegisterUserCommand : UserCommand
    {
        public RegisterUserCommand(string name, string lastName, string email, string userName, string password, byte[] salt)
        {
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
            this.Password = password;
            this.Salt = salt;
            this.UserSecurityroles = new List<UserSecurityrole>();
        }
    }
}
