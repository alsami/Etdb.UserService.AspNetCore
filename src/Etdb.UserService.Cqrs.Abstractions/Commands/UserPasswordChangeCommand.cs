using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserPasswordChangeCommand : PasswordCommand, IVoidCommand
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