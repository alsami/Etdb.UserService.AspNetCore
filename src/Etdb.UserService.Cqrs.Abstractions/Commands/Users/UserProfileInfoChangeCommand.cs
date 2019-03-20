using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserProfileInfoChangeCommand : IVoidCommand
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