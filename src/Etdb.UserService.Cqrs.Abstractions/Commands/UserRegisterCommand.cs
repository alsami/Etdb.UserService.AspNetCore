using System.Collections.Generic;
using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserRegisterCommand : UserNameCommand
    {
        public string FirstName { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public ICollection<EmailAddCommand> Emails { get; set; }
    }
}