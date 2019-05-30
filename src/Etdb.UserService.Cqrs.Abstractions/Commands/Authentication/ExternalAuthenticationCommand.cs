using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Authentication
{
    public class ExternalAuthenticationCommand : AuthenticationCommand
    {
        public string Token { get; }


        public ExternalAuthenticationCommand(string clientId, string token,
            string authenticationProvider) : base(clientId, authenticationProvider)
        {
            this.Token = token;
        }
    }
}