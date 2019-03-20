using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Misc.Configuration;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.Handler.Authentication
{
    public sealed class RefreshAuthenticationCommandHandler : AuthenticationCommandHandler<RefreshAuthenticationCommand>
    {
        public RefreshAuthenticationCommandHandler(IHttpClientFactory httpClientFactory,
            IOptions<IdentityServerConfiguration> identityServerOptions) : base(httpClientFactory,
            identityServerOptions)
        {
        }

        protected override Task<TokenResponse> RequestTokenAsync(RefreshAuthenticationCommand command,
            HttpClient client, Client identityClient,
            DiscoveryResponse discoveryResponse, CancellationToken cancellationToken = default)
            => client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                RefreshToken = command.RefreshToken,
                ClientId = identityClient.Id,
                ClientSecret = identityClient.Secret
            }, cancellationToken);
    }
}