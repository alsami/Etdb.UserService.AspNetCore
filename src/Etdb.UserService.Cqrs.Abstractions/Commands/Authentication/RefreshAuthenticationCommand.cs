using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Authentication
{
    public class RefreshAuthenticationCommand : AuthenticationCommand
    {
        public string RefreshToken { get; }

        public RefreshAuthenticationCommand(string refreshToken, string clientId, string authenticationProvider) : base(clientId, authenticationProvider)
        {
            this.RefreshToken = refreshToken;
        }
    }
}