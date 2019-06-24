using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserNameChangeCommand : UserNameCommand, IVoidCommand
    {
        public UserNameChangeCommand(Guid id, string wantedUserName) : base(id, wantedUserName)
        {
        }
    }
}