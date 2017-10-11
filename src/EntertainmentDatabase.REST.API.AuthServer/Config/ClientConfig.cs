using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertainmentDatabase.REST.API.AuthServer.Config
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
                        "EntertainmentDatabase.REST.API"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:4200"
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 60 * 60
                }
            };
        }
    }
}
