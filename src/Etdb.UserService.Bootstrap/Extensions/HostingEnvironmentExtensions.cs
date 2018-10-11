using Microsoft.AspNetCore.Hosting;

namespace Etdb.UserService.Bootstrap.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsLocalDevelopment(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName == "Development-Local";
        }
    }
}