using System;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserProfileInfoChangeCommand : IRequest
    {
        public UserProfileInfoChangeCommand(Guid id, string firstName, string name, string biography)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.Name = name;
            this.Biography = biography;
        }

        public Guid Id { get; }

        public string FirstName { get; }

        public string Name { get; }

        public string Biography { get; }
    }
}