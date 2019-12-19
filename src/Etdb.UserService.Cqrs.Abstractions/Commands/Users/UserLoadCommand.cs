using System;
using Etdb.UserService.Presentation.Users;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserLoadCommand : IRequest<UserDto>
    {
        public UserLoadCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}