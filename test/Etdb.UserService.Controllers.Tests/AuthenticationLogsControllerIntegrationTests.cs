using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Etdb.UserService.Controllers.Tests.Common;
using Etdb.UserService.Controllers.Tests.Fixtures;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Presentation.Users;
using IdentityModel.Client;
using Xunit;

namespace Etdb.UserService.Controllers.Tests
{
    public class AuthenticationLogsControllerIntegrationTests : ControllerIntegrationTests
    {
        public AuthenticationLogsControllerIntegrationTests(ConfigurationFixture configurationFixture,
            TestServerFixture testServerFixture) : base(configurationFixture, testServerFixture)
        {
        }

        [Fact]
        public async Task AuthenticationLogsController_LoadAsync_Single_Authentication_Returns_One_AuthenticationLog()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (_, accessToken, identityUser) = await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            client.SetBearerToken(accessToken.AccessToken);

            var authenticationLogsLoadResponse =
                await client.GetAsync($"api/v1/users/{identityUser.Id}/authentication-logs");

            Assert.True(authenticationLogsLoadResponse.IsSuccessStatusCode,
                await authenticationLogsLoadResponse.Content.ReadAsStringAsync());

            var authenticationLogs = await authenticationLogsLoadResponse.Content.ReadAsAsync<AuthenticationLogDto[]>();

            Assert.Single(authenticationLogs);
            Assert.Equal(authenticationLogs.First().AuthenticationLogType, AuthenticationLogType.Succeeded.ToString(),
                StringComparer.InvariantCultureIgnoreCase);
        }


        private async Task<(UserRegisterDto, TokenResponse, IdentityUserDto)>
            RegisterAuthenticateAndLoadIdentityUserAsync(HttpClient client)
        {
            var registerDto = await RegisterAssertedAsync(client);

            var accessTokenDto = await this.GetTokenAsync(registerDto);

            var identityUserDto = await LoadIdentityUserAssertedAsync(accessTokenDto.AccessToken, client);

            return (registerDto, accessTokenDto, identityUserDto);
        }
    }
}