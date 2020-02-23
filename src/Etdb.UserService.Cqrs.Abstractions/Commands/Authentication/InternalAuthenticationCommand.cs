using System.Net;
using Etdb.UserService.Cqrs.Abstractions.Base;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Authentication
{
    public class InternalAuthenticationCommand : AuthenticationCommand
    {
        public string Username { get; }
        public string Password { get; }

        public IPAddress IpAddress { get; }

        public InternalAuthenticationCommand(string username, string password, string clientId,
            string authenticationProvider, IPAddress? ipAddress) : base(clientId, authenticationProvider)
        {
            this.Username = username;
            this.Password = password;
            this.IpAddress = ipAddress ?? IPAddress.Parse("127.0.0.1");
        }
    }
}