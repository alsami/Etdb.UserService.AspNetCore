using System.Collections.Generic;
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
    public sealed class
        ExternalAuthenticationCommandHandler : AuthenticationCommandHandler<ExternalAuthenticationCommand>
    {
        private const string TokenKey = "token";
        private const string ProviderKey = "provider";
        private const string ScopeKey = "scope";

        public ExternalAuthenticationCommandHandler(IIdentityServerClient identityServerClient,
            IOptions<IdentityServerConfiguration> identityServerOptions) : base(identityServerClient,
            identityServerOptions)
        {
        }

        protected override Task<TokenResponse> RequestTokenAsync(ExternalAuthenticationCommand command,
            HttpClient client,
            Client identityClient,
            DiscoveryDocumentResponse discoveryResponse, CancellationToken cancellationToken = default)
            => client.RequestTokenAsync(new TokenRequest
            {
                ClientId = identityClient.Id,
                ClientSecret = identityClient.Secret,
                GrantType = Etdb.UserService.Misc.Constants.Identity.ExternalGrantType,
                Address = discoveryResponse.TokenEndpoint,
                Parameters = new Dictionary<string, string>
                {
                    {ExternalAuthenticationCommandHandler.TokenKey, command.Token},
                    {ExternalAuthenticationCommandHandler.ProviderKey, command.AuthenticationProvider},
                    {ExternalAuthenticationCommandHandler.ScopeKey, string.Join(" ", identityClient.Scopes)},
                    {"IpAddress", command.IpAddress.ToString()}
                },
            }, cancellationToken);
    }
}