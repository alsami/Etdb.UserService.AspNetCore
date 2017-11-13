using System.Collections.Generic;
using IdentityServer4.Models;

namespace EntertainmentDatabase.REST.API.AuthService.Bootstrap.Config
{
    public class ApiResourceConfig
    {
        public IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("EntertainmentDatabase.REST.API")
            };
        }
    }
}
