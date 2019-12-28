using System.Net;
using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Authentication
{
    public class ExternalAuthenticationCommand : AuthenticationCommand
    {
        public string Token { get; }

        public IPAddress IpAddress { get; }

        public ExternalAuthenticationCommand(string clientId, string token,
            string authenticationProvider, IPAddress? ipAddress) : base(clientId, authenticationProvider)
        {
            this.Token = token;
            this.IpAddress = ipAddress ?? IPAddress.Parse("127.0.0.1");
        }

    }
}