using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class AuthenticationCommand : IResponseCommand<AccessTokenDto>
    {
        public string ClientId { get; }

        protected AuthenticationCommand(string clientId)
        {
            this.ClientId = clientId;
        }
    }
}