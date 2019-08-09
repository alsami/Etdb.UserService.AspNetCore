using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Services.Abstractions;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.CommandHandler.Authentication
{
    public sealed class RefreshAuthenticationCommandHandler : AuthenticationCommandHandler<RefreshAuthenticationCommand>
    {
        public RefreshAuthenticationCommandHandler(IIdentityServerClient identityServerClient,
            IOptions<IdentityServerConfiguration> identityServerOptions) : base(identityServerClient,
            identityServerOptions)
        {
        }

        protected override Task<TokenResponse> RequestTokenAsync(RefreshAuthenticationCommand command,
            HttpClient client, Client identityClient,
            DiscoveryDocumentResponse discoveryResponse, CancellationToken cancellationToken = default)
            => client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                RefreshToken = command.RefreshToken,
                ClientId = identityClient.Id,
                ClientSecret = identityClient.Secret
            }, cancellationToken);
    }
}