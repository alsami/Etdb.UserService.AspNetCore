using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserLoadCommand : IResponseCommand<UserDto>
    {
        public UserLoadCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}