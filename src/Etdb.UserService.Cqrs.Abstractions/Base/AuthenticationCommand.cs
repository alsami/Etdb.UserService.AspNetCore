using System.Net;
using System.Text;
using Etdb.UserService.Presentation.Authentication;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class AuthenticationCommand : IRequest<AccessTokenDto>
    {
        public string ClientId { get; }

        public string AuthenticationProvider { get; }

        protected AuthenticationCommand(string clientId, string authenticationProvider)
        {
            this.ClientId = clientId;
            this.AuthenticationProvider = authenticationProvider;
        }
    }
}