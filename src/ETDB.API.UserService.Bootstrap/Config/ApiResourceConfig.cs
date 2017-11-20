using System.Collections.Generic;
using IdentityServer4.Models;
using ETDB.API.ServiceBase.Constants;

namespace ETDB.API.UserService.Bootstrap.Config
{
    public class ApiResourceConfig
    {
        public IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource(ServiceNames.UserService),
                new ApiResource(ServiceNames.WebService),
                new ApiResource(ServiceNames.FileService),
            };
        }
    }
}
