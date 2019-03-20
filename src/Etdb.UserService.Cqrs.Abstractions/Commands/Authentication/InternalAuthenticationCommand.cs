using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Authentication
{
    public class InternalAuthenticationCommand : AuthenticationCommand
    {
        public string Username { get; }
        public string Password { get; }

        public InternalAuthenticationCommand(string username, string password, string clientId) : base(clientId)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}