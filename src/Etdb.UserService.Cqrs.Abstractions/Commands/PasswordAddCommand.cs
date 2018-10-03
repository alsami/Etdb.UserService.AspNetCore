using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class PasswordAddCommand : PasswordCommand
    {
        public PasswordAddCommand(string password) : base(password)
        {
        }
    }
}