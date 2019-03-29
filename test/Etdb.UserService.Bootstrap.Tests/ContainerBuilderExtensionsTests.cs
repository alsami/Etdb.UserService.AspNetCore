using System.Threading.Tasks;
using Autofac;
using Etdb.UserService.Bootstrap.Extensions;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Etdb.UserService.Bootstrap.Tests
{
    public class ContainerBuilderExtensionsTests
    {
        [Fact]
        public async Task Test1()
        {
            var configuration = new ConfigurationBuilder()
                .Build();

            var env = new HostingEnvironment();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.SetupDependencies(env, configuration);

            var container = containerBuilder.Build();

            Assert.True(container.IsRegistered<IExtensionGrantValidator>(), "not registered");

            var x = container.Resolve<IExtensionGrantValidator>();

            await x.ValidateAsync(new ExtensionGrantValidationContext());
        }
    }
}