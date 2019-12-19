using System;
using Etdb.UserService.Cqrs.Abstractions.Base;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserPasswordChangeCommand : PasswordCommand, IRequest
    {
        public UserPasswordChangeCommand(Guid id, string newPassword, string currentPassword) : base(newPassword)
        {
            this.Id = id;
            this.CurrentPassword = currentPassword;
        }

        public Guid Id { get; }

        public string CurrentPassword { get; }
    }
}