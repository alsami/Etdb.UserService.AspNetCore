using System.Collections.Generic;
using Etdb.ServiceBase.Constants;
using IdentityServer4.Models;

namespace Etdb.UserService.Application.Config
{
    public class ApiResourceConfig
    {
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource(ServiceNames.UserService),
                new ApiResource(ServiceNames.StorageService),
                new ApiResource(ServiceNames.FileService),
                new ApiResource(ServiceNames.IndexService),
            };
        }
    }
}
