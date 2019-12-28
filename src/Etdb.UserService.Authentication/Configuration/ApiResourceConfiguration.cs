using System.Collections.Generic;
using Etdb.ServiceBase.Constants;
using IdentityServer4.Models;

namespace Etdb.UserService.Authentication.Configuration
{
    public class ApiResourceConfiguration
    {
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource(ServiceNames.UserService),
                new ApiResource(ServiceNames.StorageService),
                new ApiResource(ServiceNames.MessagingService),
                new ApiResource(ServiceNames.IndexService),
                new ApiResource(ServiceNames.ReportingService),
            };
        }
    }
}