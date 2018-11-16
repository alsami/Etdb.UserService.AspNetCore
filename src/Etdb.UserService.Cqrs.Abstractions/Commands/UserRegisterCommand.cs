using System;
using System.Collections.Generic;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserRegisterCommand : UserNameCommand, IResponseCommand<UserDto>
    {
        public UserRegisterCommand(string userName, string firstName, string name, ICollection<EmailAddCommand> emails,
            int loginProvider, PasswordAddCommand passwordAddCommand = null) : base(
            userName)
        {
            this.FirstName = firstName;
            this.Name = name;
            this.Emails = emails;
            this.LoginProvider = loginProvider;
            this.PasswordAddCommand = passwordAddCommand;
        }

        public string FirstName { get; }

        public string Name { get; }


        public ICollection<EmailAddCommand> Emails { get; }

        public int LoginProvider { get; }

        public PasswordAddCommand PasswordAddCommand { get; }
    }
}