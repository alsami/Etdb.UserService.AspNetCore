using System.Collections.Generic;
using Etdb.ServiceBase.Constants;
using IdentityServer4;
using IdentityServer4.Models;

namespace Etdb.UserService.Application.Config
{
    public class ClientConfig
    {
        public static IEnumerable<Client> GetClients(string clientId, string clientSecrect, string[] allowedOrigins)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = clientId,

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret(clientSecrect.Sha256())
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        ServiceNames.UserService,
                        ServiceNames.StorageService,
                        ServiceNames.FileService,
                        ServiceNames.IndexService,
                    },

                    AllowedCorsOrigins = allowedOrigins,

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
