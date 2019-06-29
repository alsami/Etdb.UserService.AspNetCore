using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Bootstrap.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsLocalDevelopment(this IHostingEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "Development-Local";

        public static bool IsAnyLocalDevelopment(this IHostingEnvironment hostingEnvironment)
            => hostingEnvironment.IsLocalDevelopment() || hostingEnvironment.IsDevelopment();

        public static bool IsAzureDevelopment(this IHostingEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "AzureDev";
    }
}