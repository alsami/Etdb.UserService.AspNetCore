using System.Collections.Generic;
using IdentityServer4.Models;

namespace Etdb.UserService.Application.Config
{
    public class IdentityResourceConfig
    {
        public IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
