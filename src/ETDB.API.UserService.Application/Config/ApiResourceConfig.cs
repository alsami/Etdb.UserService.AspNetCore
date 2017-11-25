using System.Collections.Generic;
using ETDB.API.ServiceBase.Constants;
using IdentityServer4.Models;

namespace ETDB.API.UserService.Application.Config
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
