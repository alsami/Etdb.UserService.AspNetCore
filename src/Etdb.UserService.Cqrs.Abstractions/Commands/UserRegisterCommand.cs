using System;
using System.Collections.Generic;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserRegisterCommand : IResponseCommand<UserDto>
    {
        public string UserName { get; set; }
        
        public string FirstName { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public ICollection<EmailAddCommand> Emails { get; set; }
    }
}