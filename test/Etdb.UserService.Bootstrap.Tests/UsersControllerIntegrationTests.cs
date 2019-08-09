using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Etdb.UserService.Bootstrap.Tests.Common;
using Etdb.UserService.Bootstrap.Tests.Fixtures;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Presentation.Users;
using IdentityModel.Client;
using Newtonsoft.Json;
using Xunit;

namespace Etdb.UserService.Bootstrap.Tests
{
    public class UsersControllerIntegrationTests : ControllerIntegrationTests
    {
        public UsersControllerIntegrationTests(ConfigurationFixture configurationFixture,
            TestServerFixture testServerFixture) : base(configurationFixture, testServerFixture)
        {
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("&/21321")]
        [InlineData("anton_bertta-121")]
        [InlineData("joseph-maria.stift")]
        [InlineData("sadsadasdasd")]
        [InlineData("sadsadsadsadsss")]
        public async Task UsersController_AvailabilityAsync_Available_UserName_Succeeds(string userName)
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var url = $"api/v1/users/availability/{HttpUtility.UrlEncode(userName)}";

            var availabilityResponse =
                await httpClient.GetAsync(url);

            Assert.True(availabilityResponse.IsSuccessStatusCode,
                "user-name availability check was expected to be successful!");
            Assert.Equal(HttpStatusCode.OK, availabilityResponse.StatusCode);
            Assert.True(JsonConvert
                .DeserializeObject<UserNameAvailabilityDto>(await availabilityResponse.Content.ReadAsStringAsync())
                .Available, "Response flag was expected to be set to true but is set to false!");
        }

        [Fact]
        public async Task UsersController_AvailabilityAsync_Unavailable_UserName_Fails()
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var registrationDto = CreateRandomRegistrationDto();
            await RegisterAsync(registrationDto, httpClient);

            var availabilityResponse =
                await httpClient.GetAsync($"api/v1/users/availability/{registrationDto.UserName}");

            Assert.True(availabilityResponse.IsSuccessStatusCode,
                "user-name availability check was expected to be successful!");
            Assert.Equal(HttpStatusCode.OK, availabilityResponse.StatusCode);
            Assert.False(JsonConvert
                .DeserializeObject<UserNameAvailabilityDto>(await availabilityResponse.Content.ReadAsStringAsync())
                .Available, "Response flag was expected to be set to false but is set to true!");
        }

        [Fact]
        public async Task UsersController_LoadAsync_Valid_User_Returns_User()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (registerDto, accessTokenDto, identityUserDto) =
                await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            client.SetBearerToken(accessTokenDto.AccessToken);

            var userLoadResponse = await client.GetAsync($"api/v1/users/{identityUserDto.Id}");

            Assert.True(userLoadResponse.IsSuccessStatusCode, await userLoadResponse.Content.ReadAsStringAsync());

            var userDto = await userLoadResponse.Content.ReadAsAsync<UserDto>();

            Assert.Equal(registerDto.FirstName, userDto.FirstName);
            Assert.Equal(registerDto.Name, userDto.Name);
            Assert.Equal(registerDto.UserName, userDto.UserName);
        }

        [Fact]
        public async Task UsersController_UserNameChangeAsync_Valid_UserName_Succeeds()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (_, accessTokenDto, identityUserDto) = await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            client.SetBearerToken(accessTokenDto.AccessToken);

            var newUserName = Guid.NewGuid().ToString().Replace("-", "");

            var userNameChangeResponse =
                await client.PatchAsync($"api/v1/users/{identityUserDto.Id}/username/{newUserName}", null);

            Assert.True(userNameChangeResponse.IsSuccessStatusCode,
                await userNameChangeResponse.Content.ReadAsStringAsync());

            var userLoadResponse = await client.GetAsync($"api/v1/users/{identityUserDto.Id}");

            var userDto = await userLoadResponse.Content.ReadAsAsync<UserDto>();

            Assert.Equal(newUserName, userDto.UserName, StringComparer.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task UsersController_PasswordChangeAsync_Old_Password_Valid_New_Passwords_Match_Succeeds()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (registerDto, accessTokenDto, identityUserDto) =
                await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            client.SetBearerToken(accessTokenDto.AccessToken);

            var userPasswordChangeDto =
                new UserPasswordChangeDto(registerDto.Password, Guid.NewGuid().ToString().Replace("-", ""));

            var userPasswordChangeResponse = await client.PatchAsync($"api/v1/users/{identityUserDto.Id}/password",
                new StringContent(JsonConvert.SerializeObject(userPasswordChangeDto), Encoding.UTF8,
                    "application/json"));

            Assert.True(userPasswordChangeResponse.IsSuccessStatusCode,
                await userPasswordChangeResponse.Content.ReadAsStringAsync());

            var authenticationResponse = await AuthenticateAsync(
                new InternalAuthenticationDto(registerDto.UserName, userPasswordChangeDto.NewPassword,
                    this.GetClientId()), client);

            Assert.True(authenticationResponse.IsSuccessStatusCode,
                await authenticationResponse.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task UsersController_ProfileInfoChangeAsync_Succeeds()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (_, accessTokenDto, identityUserDto) =
                await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            client.SetBearerToken(accessTokenDto.AccessToken);

            var profileChangeDto = new UserProfileInfoChangeDto("newFirstName", "newLastName", "Biography");

            var profileInfoChangeResponse = await client.PatchAsync($"api/v1/users/{identityUserDto.Id}/profileinfo",
                new StringContent(JsonConvert.SerializeObject(profileChangeDto), Encoding.UTF8, "application/json"));

            Assert.True(profileInfoChangeResponse.IsSuccessStatusCode,
                await profileInfoChangeResponse.Content.ReadAsStringAsync());

            var userLoadResponse = await client.GetAsync($"api/v1/users/{identityUserDto.Id}");

            var userDto = await userLoadResponse.Content.ReadAsAsync<UserDto>();

            Assert.Equal(profileChangeDto.FirstName, userDto.FirstName, StringComparer.InvariantCultureIgnoreCase);
            Assert.Equal(profileChangeDto.Name, userDto.Name, StringComparer.InvariantCultureIgnoreCase);
            Assert.Equal(profileChangeDto.Biography, userDto.Biography, StringComparer.InvariantCultureIgnoreCase);
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