using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Authentication
{
    public class ExternalAuthenticationCommand : AuthenticationCommand
    {
        public string Token { get; }

        public string Provider { get; }

        public ExternalAuthenticationCommand(string clientId, string token,
            string provider) : base(clientId)
        {
            this.Token = token;
            this.Provider = provider;
        }
    }
}