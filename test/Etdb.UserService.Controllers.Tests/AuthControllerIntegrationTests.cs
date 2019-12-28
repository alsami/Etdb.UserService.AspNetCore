using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Authentication.Structures;
using Etdb.UserService.Controllers.Tests.Common;
using Etdb.UserService.Controllers.Tests.Fixtures;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Authentication;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Etdb.UserService.Controllers.Tests
{
    public class AuthControllerIntegrationTests : ControllerIntegrationTests
    {
        private const string SendAsyncMethodName = "SendAsync";
        private readonly ITestOutputHelper testOutputHelper;

        public AuthControllerIntegrationTests(ConfigurationFixture configurationFixture,
            TestServerFixture testServerFixture, ITestOutputHelper testOutputHelper) :
            base(configurationFixture, testServerFixture)
        {
            this.testOutputHelper = testOutputHelper;
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

            var registerDto = await RegisterAssertedAsync(httpClient);

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

            var registerDto = await RegisterAssertedAsync(httpClient);

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

            var registerDto = await RegisterAssertedAsync(httpClient);

            var tokenResponse = await this.GetTokenAsync(registerDto);
            
            Assert.False(tokenResponse.IsError, tokenResponse.Error ?? tokenResponse.ErrorDescription);

            var identityUserLoadResponse = await LoadIdentityUserAsync(tokenResponse.AccessToken, httpClient);

            var responseContent = await identityUserLoadResponse.Content.ReadAsStringAsync();

            this.testOutputHelper.WriteLine(responseContent);
            
            Assert.Equal(HttpStatusCode.OK, identityUserLoadResponse.StatusCode);

            var userProfileDto =
                JsonConvert.DeserializeObject<IdentityUserDto>(responseContent);

            Assert.Equal(registerDto.UserName, userProfileDto.UserName);
        }

        [Fact]
        public async Task AuthController_RefreshAuthenticationAsync_Valid_Token_Succeeds()
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var registerDto = await RegisterAssertedAsync(httpClient);

            var tokenResponse = await this.GetTokenAsync(registerDto);

            var refreshAuthenticationResponse =
                await this.RefreshAuthenticationAsync(tokenResponse.RefreshToken, httpClient,
                    AuthenticationProvider.UsernamePassword);

            Assert.True(refreshAuthenticationResponse.IsSuccessStatusCode,
                await refreshAuthenticationResponse.Content.ReadAsStringAsync() ??
                refreshAuthenticationResponse.StatusCode.ToString());
        }

        [Fact]
        public async Task AuthController_ExternalAuthenticationAsync_Google_Invalid_Token_Unauthorized()
        {
            var fixture = new TestServerFixture();
            var httpClient = fixture.ApiServer.CreateClient();

            var authenticationDto = new ExternalAuthenticationDto(this.GetClientId(), Guid.NewGuid().ToString(),
                AuthenticationProvider.Google.ToString());

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(
                        new StandardizedAuthErrorContainer(new StandardizedAuthError("Unknown token", 400))))
                });

            var externalAuthenticationResponse =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName, Times.Once(),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            Assert.False(externalAuthenticationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, externalAuthenticationResponse.StatusCode);
        }

        [Fact]
        public async Task AuthController_ExternalAuthenticationAsync_Google_Valid_Token_New_User_Authorized()
        {
            var fixture = new TestServerFixture();
            var httpClient = fixture.ApiServer.CreateClient();

            var authenticationDto = new ExternalAuthenticationDto(this.GetClientId(), Guid.NewGuid().ToString(),
                AuthenticationProvider.Google.ToString());

            var fakeGoogleProfile = GenerateFakeGoogleProfileResponse();

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(fakeGoogleProfile))
                });

            var externalAuthenticationResponse =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName, Times.Exactly(2),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            Assert.True(externalAuthenticationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, externalAuthenticationResponse.StatusCode);
        }

        [Fact]
        public async Task AuthController_ExternalAuthenticationAsync_Google_Valid_Token_Existing_User_Authorized()
        {
            var fixture = new TestServerFixture();
            var httpClient = fixture.ApiServer.CreateClient();

            var authenticationDto = new ExternalAuthenticationDto(this.GetClientId(), Guid.NewGuid().ToString(),
                AuthenticationProvider.Google.ToString());

            var fakeGoogleProfile = GenerateFakeGoogleProfileResponse();

            HttpResponseMessage Create() => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(fakeGoogleProfile))
            };

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(Create);

            var externalAuthenticationResponseOne =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            Assert.True(externalAuthenticationResponseOne.IsSuccessStatusCode);

            var externalAuthenticationResponseTwo =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName, Times.Exactly(3),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            Assert.True(externalAuthenticationResponseTwo.IsSuccessStatusCode,
                await externalAuthenticationResponseTwo.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task
            AuthController_ExternalAuthenticationAsync_Google_Valid_Token_Existing_User_Different_Provider_Unauthorized()
        {
            var fixture = new TestServerFixture();
            var httpClient = fixture.ApiServer.CreateClient();

            var userDto = await RegisterAssertedAsync(httpClient);

            var authenticationDto = new ExternalAuthenticationDto(this.GetClientId(), Guid.NewGuid().ToString(),
                AuthenticationProvider.Google.ToString());

            var fakeGoogleProfile = new
            {
                Email = userDto.Emails.First().Address,
                Given_Name = "Some given Name",
                Family_Name = "Some family Name",
                Picture = "https://lh6.googleusercontent.com/-UNUSGzVwNaY/AAAAAAAAAAI/AAAAAAAAgxM/lnDKDkBh370/photo.jpg"
            };

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(fakeGoogleProfile))
                });

            var externalAuthenticationResponse =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName, Times.Once(),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            Assert.False(externalAuthenticationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, externalAuthenticationResponse.StatusCode);
        }

        [Fact]
        public async Task AuthController_ExternalAuthenticationAsync_Facebook_Invalid_Token_Unauthorized()
        {
            var fixture = new TestServerFixture();
            var httpClient = fixture.ApiServer.CreateClient();

            var authenticationDto = new ExternalAuthenticationDto(this.GetClientId(), Guid.NewGuid().ToString(),
                AuthenticationProvider.Facebook.ToString());

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(
                        new StandardizedAuthErrorContainer(new StandardizedAuthError("Unknown token", 400))))
                });

            var externalAuthenticationResponse =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName, Times.Once(),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            Assert.False(externalAuthenticationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, externalAuthenticationResponse.StatusCode);
        }

        [Fact]
        public async Task AuthController_ExternalAuthenticationAsync_Facebook_Valid_Token_New_User_Authorized()
        {
            var fixture = new TestServerFixture();
            var httpClient = fixture.ApiServer.CreateClient();

            var authenticationDto = new ExternalAuthenticationDto(this.GetClientId(), Guid.NewGuid().ToString(),
                AuthenticationProvider.Facebook.ToString());

            var fakeFacebookProfile = GenerateFakeFacebookProfileResponse();

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(fakeFacebookProfile))
                });

            var externalAuthenticationResponse =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName, Times.Exactly(2),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            Assert.True(externalAuthenticationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, externalAuthenticationResponse.StatusCode);
        }

        [Fact]
        public async Task AuthController_ExternalAuthenticationAsync_Facebook_Valid_Token_Existing_User_Authorized()
        {
            var fixture = new TestServerFixture();
            var httpClient = fixture.ApiServer.CreateClient();

            var authenticationDto = new ExternalAuthenticationDto(this.GetClientId(), Guid.NewGuid().ToString(),
                AuthenticationProvider.Facebook.ToString());

            var fakeFacebookProfile = GenerateFakeFacebookProfileResponse();

            HttpResponseMessage Create() => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(fakeFacebookProfile))
            };

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(Create);

            var externalAuthenticationResponseOne =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            Assert.True(externalAuthenticationResponseOne.IsSuccessStatusCode, await externalAuthenticationResponseOne.Content.ReadAsStringAsync());

            var externalAuthenticationResponseTwo =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName, Times.Exactly(3),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            Assert.True(externalAuthenticationResponseTwo.IsSuccessStatusCode,
                await externalAuthenticationResponseTwo.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task
            AuthController_ExternalAuthenticationAsync_Facebook_Valid_Token_Existing_User_Different_Provider_Unauthorized()
        {
            var fixture = new TestServerFixture();
            var httpClient = fixture.ApiServer.CreateClient();

            var userDto = await RegisterAssertedAsync(httpClient);

            var authenticationDto = new ExternalAuthenticationDto(this.GetClientId(), Guid.NewGuid().ToString(),
                AuthenticationProvider.Google.ToString());

            var fakeFacebookProfile = new
            {
                Email = userDto.Emails.First().Address,
                Name = "SomeName SomeFamilyName",
                Picture = new
                {
                    Data = new
                    {
                        Url =
                            "https://platform-lookaside.fbsbx.com/platform/profilepic/?asid=106879457092447&height=50&width=50&ext=1567268223&hash=AeRO3sgC7n8T6YQB"
                    }
                }
            };

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName,
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(fakeFacebookProfile))
                });

            var externalAuthenticationResponse =
                await httpClient.PostAsJsonAsync("api/v1/auth/external-authentication", authenticationDto);

            fixture.ExternalIdentityHttpMessageHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(AuthControllerIntegrationTests.SendAsyncMethodName, Times.Once(),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            Assert.False(externalAuthenticationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, externalAuthenticationResponse.StatusCode);
        }

        private static object GenerateFakeGoogleProfileResponse()
            => new
            {
                Email = $"{Guid.NewGuid()}@gmail.com",
                Given_Name = "Some given Name",
                Family_Name = "Some family Name",
                Picture = "https://lh6.googleusercontent.com/-UNUSGzVwNaY/AAAAAAAAAAI/AAAAAAAAgxM/lnDKDkBh370/photo.jpg"
            };

        private static object GenerateFakeFacebookProfileResponse()
            => new
            {
                Email = $"{Guid.NewGuid()}@facebook.com",
                Name = "SomeName SomeFamilyName",
                Picture = new
                {
                    Data = new
                    {
                        Url =
                            "https://platform-lookaside.fbsbx.com/platform/profilepic/?asid=106879457092447&height=50&width=50&ext=1567268223&hash=AeRO3sgC7n8T6YQB"
                    }
                }
            };
    }
}