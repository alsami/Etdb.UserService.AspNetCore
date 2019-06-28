using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsLocalDevelopment(this IHostingEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "Development-Local";

        public static bool IsAzureDevelopment(this IHostingEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "AzureDev";
    }
}