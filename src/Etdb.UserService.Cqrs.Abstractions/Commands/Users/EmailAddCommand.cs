using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class EmailAddCommand : EmailCommand
    {
        public EmailAddCommand(string address, bool isPrimary, bool isExternal) : base(address, isPrimary, isExternal)
        {
        }
    }
}