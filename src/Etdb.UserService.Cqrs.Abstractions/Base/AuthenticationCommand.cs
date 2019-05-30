using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class AuthenticationCommand : IResponseCommand<AccessTokenDto>
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