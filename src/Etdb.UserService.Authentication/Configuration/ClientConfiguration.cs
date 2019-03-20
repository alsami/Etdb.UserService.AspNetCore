using System.Collections.Generic;
using System.Linq;
using Etdb.UserService.Misc.Configuration;
using IdentityServer4.Models;
using Client = IdentityServer4.Models.Client;

namespace Etdb.UserService.Authentication.Configuration
{
    public class ClientConfiguration
    {
        public static IEnumerable<Client> GetClients(IdentityServerConfiguration configuration)
        {
            var identityClients = configuration.Clients.Select(client => new Client
            {
                ClientId = client.Id,
                AllowedGrantTypes = client.GrantTypes,
                AllowedScopes = client.Scopes,
                ClientSecrets =
                {
                    new Secret(client.Secret.Sha256())
                },
                AllowOfflineAccess = client.HasOfflineAccess,
                AllowedCorsOrigins = client.Origins,
                AccessTokenLifetime = 60 * 60,
                AlwaysSendClientClaims = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Absolute,
            }).ToArray();

            return identityClients;
        }
    }
}