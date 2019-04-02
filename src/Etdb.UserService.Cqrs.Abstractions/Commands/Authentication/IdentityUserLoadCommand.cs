using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Authentication
{
    public class IdentityUserLoadCommand : IResponseCommand<IdentityUserDto>
    {
        public string AccessToken { get; }

        public IdentityUserLoadCommand(string accessToken)
        {
            this.AccessToken = accessToken;
        }
    }
}