using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Etdb.UserService.Controllers.Tests.Fixtures
{
    public class ConfigurationFixture
    {
        public IConfiguration Configuration { get; }

        public ConfigurationFixture()
        {
            this.Configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.Development.json"))
                .AddEnvironmentVariables()
                .AddUserSecrets("Etdb_UserService")
                .Build();
        }
    }
}