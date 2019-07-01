using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Misc.Exceptions;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Services.Abstractions;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.CommandHandler.Authentication
{
    public abstract class AuthenticationCommandHandler<TCommand> : IResponseCommandHandler<TCommand, AccessTokenDto>
        where TCommand : AuthenticationCommand
    {
        private readonly IIdentityServerClient identityServerClient;
        private readonly IOptions<IdentityServerConfiguration> identityServerOptions;

        protected AuthenticationCommandHandler(IIdentityServerClient identityServerClient,
            IOptions<IdentityServerConfiguration> identityServerOptions)
        {
            this.identityServerClient = identityServerClient;
            this.identityServerOptions = identityServerOptions;
        }

        public async Task<AccessTokenDto> Handle(TCommand command, CancellationToken cancellationToken)
        {
            if (!this.TryFindClient(command, out var identityClient))
            {
                throw InvalidClientId(command);
            }

            var client = this.identityServerClient.Client;

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
                DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn), tokenResponse.TokenType,
                command.AuthenticationProvider);
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