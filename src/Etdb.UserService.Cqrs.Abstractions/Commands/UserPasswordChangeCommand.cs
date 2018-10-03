using System;
using System.Collections.Generic;
using System.Text;
using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserPasswordChangeCommand : PasswordCommand
    {
        public UserPasswordChangeCommand(string newPassword, string currentPassword) : base(newPassword)
        {
            CurrentPassword = currentPassword;
        }

        public string CurrentPassword { get; }
    }
}