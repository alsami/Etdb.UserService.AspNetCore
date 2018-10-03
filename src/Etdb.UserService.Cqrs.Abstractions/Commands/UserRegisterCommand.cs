using System.Collections.Generic;
using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserRegisterCommand : UserNameCommand
    {
        public UserRegisterCommand(string userName, string firstName, string name,
            PasswordAddCommand passwordAddCommand,
            ICollection<EmailAddCommand> emails) : base(userName)
        {
            this.FirstName = firstName;
            this.Name = name;
            this.PasswordAddCommand = passwordAddCommand;
            this.Emails = emails;
        }

        public string FirstName { get; }

        public string Name { get; }

        public PasswordAddCommand PasswordAddCommand { get; set; }

        public ICollection<EmailAddCommand> Emails { get; }
    }
}