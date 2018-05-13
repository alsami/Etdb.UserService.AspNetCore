﻿using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;

namespace Etdb.UserService.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsLocalDevelopment(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName == "Development-Local";
        }
    }
}