using System;
using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsLocalDevelopment(this IHostEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "Development-Local";

        public static bool IsAnyLocalDevelopment(this IHostEnvironment hostingEnvironment)
            => hostingEnvironment.IsLocalDevelopment() || hostingEnvironment.IsDevelopment();

        public static bool IsAzureDevelopment(this IHostEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "AzureDev";

        public static bool IsClientGen(this IHostEnvironment hostEnvironment)
            => hostEnvironment.EnvironmentName.Equals("ClientGen", StringComparison.InvariantCultureIgnoreCase);
    }
}