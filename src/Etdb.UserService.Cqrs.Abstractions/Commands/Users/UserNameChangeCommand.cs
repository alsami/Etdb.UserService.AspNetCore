using System;
using Etdb.UserService.Cqrs.Abstractions.Base;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserNameChangeCommand : UserNameCommand, IRequest
    {
        public UserNameChangeCommand(Guid id, string wantedUserName) : base(id, wantedUserName)
        {
        }
    }
}