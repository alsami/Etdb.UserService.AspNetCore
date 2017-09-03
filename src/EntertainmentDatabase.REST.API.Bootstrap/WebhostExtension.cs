using EntertainmentDatabase.REST.API.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertainmentDatabase.REST.API.Bootstrap
{
    public static class WebhostExtension
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var db = services.GetRequiredService<EntertainmentDatabaseContext>();
                    db.Database.Migrate();
                }
                catch (Exception)
                {
                    throw;
                    //var logger = services.GetRequiredService<ILogger<Program>>();
                    //logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return webHost;
        }
    }
}
