using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class PasswordAddCommand : PasswordCommand
    {
        public PasswordAddCommand(string newPassword) : base(newPassword)
        {
        }
    }
}