using Etdb.UserService.Bootstrap.Tests.Common;
using Etdb.UserService.Bootstrap.Tests.Fixtures;

namespace Etdb.UserService.Bootstrap.Tests
{
    public class UsersControllerIntegrationTests : ControllerIntegrationTests
    {
        public UsersControllerIntegrationTests(ConfigurationFixture configurationFixture, TestServerFixture testServerFixture) : base(configurationFixture, testServerFixture)
        {
        }
    }
}
