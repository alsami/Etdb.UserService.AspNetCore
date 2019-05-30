using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.CommandHandler.Authentication
{
    public sealed class
        InternalAuthenticationCommandHandler : AuthenticationCommandHandler<InternalAuthenticationCommand>
    {
        public InternalAuthenticationCommandHandler(IIdentityServerClient identityServerClient,
            IOptions<IdentityServerConfiguration> identityServerOptions) : base(identityServerClient,
            identityServerOptions)
        {
        }

        protected override Task<TokenResponse> RequestTokenAsync(InternalAuthenticationCommand command,
            HttpClient client, Client identityClient,
            DiscoveryResponse discoveryResponse, CancellationToken cancellationToken = default)
            => client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                ClientId = identityClient.Id,
                ClientSecret = identityClient.Secret,
                GrantType = OidcConstants.GrantTypes.Password,
                Scope = string.Join(" ", identityClient.Scopes),
                UserName = command.Username,
                Password = command.Password,
                Address = discoveryResponse.TokenEndpoint
            }, cancellationToken);
    }
}