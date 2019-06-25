using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Authentication.Structures;
using Etdb.UserService.Bootstrap.Tests.Common;
using Etdb.UserService.Bootstrap.Tests.Fixtures;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Authentication;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Etdb.UserService.Bootstrap.Tests
{
    public class AuthControllerIntegrationTests : ControllerIntegrationTests
    {
        private const string SendAsyncMethodName = "SendAsync";

        public AuthControllerIntegrationTests(ConfigurationFixture configurationFixture, TestServerFixture testServerFixture) :
            base(configurationFixture, testServerFixture)
        {
        }

        [Fact]
        public async Task AuthController_RegistrationAsync_Valid_Input_User_Registered()
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var registerDto = CreateRandomRegistrationDto();

            var registerResponse = await RegisterAsync(registerDto, httpClient);

            Assert.True(registerResponse.IsSuccessStatusCode, await registerResponse.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.NoContent, registerResponse.StatusCode);
        }

        [Fact]
        public async Task AuthController_AuthenticationAsync_Valid_Credentials_Authenticated()
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var registerDto = await RegisterAsync(httpClient);

            var authenticationDto =
                new InternalAuthenticationDto(registerDto.UserName, registerDto.Password, this.GetClientId());

            var authenticationResponse = await AuthenticateAsync(authenticationDto, httpClient);

            Assert.True(authenticationResponse.IsSuccessStatusCode,
                await authenticationResponse.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, authenticationResponse.StatusCode);
        }

        [Fact]
        public async Task AuthController_AuthenticationAsync_Invalid_Credentials_Unauthorized()
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var registerDto = await RegisterAsync(httpClient);

            var authenticationDto = new InternalAuthenticationDto(registerDto.UserName,
                $"{registerDto.Password}ssadasdsadasdsa", this.GetClientId());

            var authenticationResponse = await AuthenticateAsync(authenticationDto, httpClient);

            Assert.False(authenticationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, authenticationResponse.StatusCode);
        }

        [Fact]
        public async Task AuthController_LoadUserInfosAsync_Valid_Credentials_Succeeds()
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var registerDto = await RegisterAsync(httpClient);

            var tokenResponse = await this.GetTokenAsync(registerDto);

            var identityUserLoadResponse = await LoadIdentityUserAsync(tokenResponse.AccessToken, httpClient);

            Assert.Equal(HttpStatusCode.OK, identityUserLoadResponse.StatusCode);

            var userProfileDto =
                JsonConvert.DeserializeObject<IdentityUserDto>(
                    await identityUserLoadResponse.Content.ReadAsStringAsync());

            Assert.Equal(registerDto.UserName, userProfileDto.UserName);
        }

        [Fact]
        public async Task AuthController_RefreshAuthenticationAsync_Valid_Token_Succeeds()
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var registerDto = await RegisterAsync(httpClient);

            var tokenResponse = await this.GetTokenAsync(registerDto);

            var refreshAuthenticationResponse =
                await this.RefreshAuthenticationAsync(tokenResponse.RefreshToken, httpClient, AuthenticationProvider.UsernamePassword);

            Assert.True(refreshAuthenticationResponse.IsSuccessStatusCode,
                await refreshAuthenticationResponse.Content.ReadAsStringAsync() ??
                refreshAuthenticationResponse.StatusCode.ToString());
        }

        [Fact]
        public async Task AuthController_ExternalAuthenticationAsync_Invalid_Token_Unauthorized()
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var authenticationDto = new ExternalAuthenticationDto(this.GetClientId(), Guid.NewGuid().ToString(),
                AuthenticationProvider.Google.ToString());

            this.TestServerFixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(
                        new StandardizedAuthErrorContainer(new StandardizedAuthError("Unknown token", 400))))
                });

            var externalAuthenticationResponse =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            try
            {
                this.TestServerFixture.ExternalIdentityHttpMessageHandlerMock
                    .Protected()
                    .Verify<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName, Times.Once(),
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>());
            }
            catch (Exception)
            {
                Assert.True(externalAuthenticationResponse.IsSuccessStatusCode,
                    await externalAuthenticationResponse.Content.ReadAsStringAsync());
                throw;
            }

            Assert.False(externalAuthenticationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, externalAuthenticationResponse.StatusCode);
        }
    }
}