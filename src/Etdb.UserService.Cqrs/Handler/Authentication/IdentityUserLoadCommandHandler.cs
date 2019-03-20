using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Misc.Exceptions;
using Etdb.UserService.Presentation;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.Handler.Authentication
{
    public class IdentityUserLoadCommandHandler : IResponseCommandHandler<IdentityUserLoadCommand, IdentityUserDto>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMapper mapper;
        private readonly IOptions<IdentityServerConfiguration> identityServerOptions;

        public IdentityUserLoadCommandHandler(IHttpClientFactory httpClientFactory,
            IOptions<IdentityServerConfiguration> identityServerOptions, IMapper mapper)
        {
            this.httpClientFactory = httpClientFactory;
            this.identityServerOptions = identityServerOptions;
            this.mapper = mapper;
        }

        public async Task<IdentityUserDto> Handle(IdentityUserLoadCommand command, CancellationToken cancellationToken)
        {
            var client = this.httpClientFactory.CreateClient();

            var discoveryDocument =
                await client.GetDiscoveryDocumentAsync(this.identityServerOptions.Value.Authority, cancellationToken);

            if (discoveryDocument.IsError)
            {
                throw new IdentityServerException(discoveryDocument.Error);
            }

            var claimResponse = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Token = command.AccessToken,
                Address = discoveryDocument.UserInfoEndpoint
            }, cancellationToken);

            if (claimResponse.IsError)
            {
                throw new IdentityServerException(claimResponse.Error);
            }

            return this.mapper.Map<IdentityUserDto>(claimResponse.Claims);
        }
    }
}