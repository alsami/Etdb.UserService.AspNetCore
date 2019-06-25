using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Etdb.UserService.Bootstrap.Tests.Common;
using Etdb.UserService.Bootstrap.Tests.Fixtures;
using Etdb.UserService.Presentation.Users;
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

        [Fact]
        public async Task UsersController_AvailabilityAsync_Available_UserName_Succeeds()
        {
            var httpClient = this.TestServerFixture.ApiServer.CreateClient();

            var availabilityResponse =
                await httpClient.GetAsync($"api/v1/users/availability/{Guid.NewGuid().ToString().Replace("-", "")}");

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
    }
}