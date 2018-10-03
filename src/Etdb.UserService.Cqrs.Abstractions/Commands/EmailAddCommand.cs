using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class EmailAddCommand : EmailCommand
    {
        public EmailAddCommand(string address, bool isPrimary) : base(address, isPrimary)
        {
        }
    }
}