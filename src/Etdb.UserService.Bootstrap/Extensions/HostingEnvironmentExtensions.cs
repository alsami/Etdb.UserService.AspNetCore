using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Bootstrap.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsLocalDevelopment(this IWebHostEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "Development-Local";

        public static bool IsAnyLocalDevelopment(this IWebHostEnvironment hostingEnvironment)
            => hostingEnvironment.IsLocalDevelopment() || hostingEnvironment.IsDevelopment();

        public static bool IsAzureDevelopment(this IWebHostEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "AzureDev";
    }
}