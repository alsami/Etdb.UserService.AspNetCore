using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Misc.Exceptions;
using Etdb.UserService.Presentation;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.Handler.Authentication
{
    public abstract class AuthenticationCommandHandler<TCommand> : IResponseCommandHandler<TCommand, AccessTokenDto>
        where TCommand : AuthenticationCommand
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOptions<IdentityServerConfiguration> identityServerOptions;

        protected AuthenticationCommandHandler(IHttpClientFactory httpClientFactory,
            IOptions<IdentityServerConfiguration> identityServerOptions)
        {
            this.httpClientFactory = httpClientFactory;
            this.identityServerOptions = identityServerOptions;
        }

        public async Task<AccessTokenDto> Handle(TCommand command, CancellationToken cancellationToken)
        {
            if (!this.TryFindClient(command, out var identityClient))
            {
                throw InvalidClientId(command);
            }

            var client = this.httpClientFactory.CreateClient();

            var discoveryDocument =
                await client.GetDiscoveryDocumentAsync(this.identityServerOptions.Value.Authority, cancellationToken);

            if (discoveryDocument.IsError)
            {
                throw new IdentityServerException(discoveryDocument.Error);
            }

            var tokenResponse =
                await this.RequestTokenAsync(command, client, identityClient, discoveryDocument, cancellationToken);

            if (tokenResponse.IsError)
            {
                throw new IdentityServerException(tokenResponse.ErrorDescription ?? tokenResponse.Error);
            }

            return new AccessTokenDto(tokenResponse.AccessToken, tokenResponse.RefreshToken,
                DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn), tokenResponse.TokenType);
        }

        protected abstract Task<TokenResponse> RequestTokenAsync(TCommand command, HttpClient client,
            Client identityClient,
            DiscoveryResponse discoveryResponse, CancellationToken cancellationToken = default);

        private bool TryFindClient(TCommand command, out Client client)
        {
            client = this.identityServerOptions.Value.Clients.SingleOrDefault(existingClient =>
                existingClient.Id == command.ClientId);

            return client != null;
        }

        private static IdentityServerException InvalidClientId(TCommand command)
            => new IdentityServerException($"Client {command.ClientId} is invalid client");
    }
}