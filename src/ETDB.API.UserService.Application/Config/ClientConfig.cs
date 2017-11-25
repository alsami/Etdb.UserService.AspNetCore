using System.Collections.Generic;
using ETDB.API.ServiceBase.Constants;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace ETDB.API.UserService.Application.Config
{
    public class ClientConfig
    {
        public IEnumerable<Client> GetClients(IConfigurationRoot configuration)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = configuration.GetSection("IdentityConfig")
                        .GetSection("WebClient")
                        .GetValue<string>("Name"),

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret(configuration.GetSection("IdentityConfig")
                            .GetSection("WebClient")
                            .GetValue<string>("Secret").Sha256())
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        ServiceNames.UserService,
                        ServiceNames.WebService,
                        ServiceNames.FileService,
                    },

                    AllowedCorsOrigins = configuration.GetSection("IdentityConfig")
                        .GetSection("Origins")
                        .Get<string[]>(),

                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 60 * 60,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                }
            };
        }
    }
}
