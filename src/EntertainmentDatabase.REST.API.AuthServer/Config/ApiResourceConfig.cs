using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertainmentDatabase.REST.API.AuthServer.Config
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
