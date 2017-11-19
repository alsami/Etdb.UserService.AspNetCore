using System.Collections.Generic;
using ETDB.API.ServiceBase.Constants;
using IdentityServer4;
using IdentityServer4.Models;

namespace ETDB.API.UserService.Bootstrap.Config
{
    public class ClientConfig
    {
        public IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "web.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
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
                    AllowedCorsOrigins =
                    {
                        "http://localhost:4200"
                    },
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
