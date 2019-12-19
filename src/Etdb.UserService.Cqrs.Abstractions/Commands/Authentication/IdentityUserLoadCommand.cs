using Etdb.UserService.Presentation.Authentication;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Authentication
{
    public class IdentityUserLoadCommand : IRequest<IdentityUserDto>
    {
        public string AccessToken { get; }

        public IdentityUserLoadCommand(string accessToken)
        {
            this.AccessToken = accessToken;
        }
    }
}