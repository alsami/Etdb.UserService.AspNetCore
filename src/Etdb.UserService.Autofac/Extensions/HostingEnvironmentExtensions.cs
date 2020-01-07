using System;
using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Autofac.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsAnyDevelopment(this IHostEnvironment hostingEnvironment)
            => hostingEnvironment.IsLocalDevelopment() || 
               hostingEnvironment.IsDevelopment() || 
               hostingEnvironment.IsContinousIntegration();

        public static bool IsLocalDevelopment(this IHostEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "Development-Local";
        
        public static bool IsAzureDevelopment(this IHostEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "AzureDev";

        public static bool IsContinousIntegration(this IHostEnvironment hostEnvironment)
            => hostEnvironment.EnvironmentName == "CI";

        public static bool IsClientGen(this IHostEnvironment hostEnvironment)
            => hostEnvironment.EnvironmentName.Equals("ClientGen", StringComparison.InvariantCultureIgnoreCase);
    }
}