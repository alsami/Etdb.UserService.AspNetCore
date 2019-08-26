using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Etdb.UserService.Controllers.Tests.Common;
using Etdb.UserService.Controllers.Tests.Fixtures;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Presentation.Users;
using IdentityModel.Client;
using Xunit;
using Xunit.Abstractions;

namespace Etdb.UserService.Controllers.Tests
{
    public class ProfileImagesControllerIntegrationTests : ControllerIntegrationTests
    {
        private readonly string imageFilesBasePath;
        private readonly ITestOutputHelper testOutputHelper;

        public ProfileImagesControllerIntegrationTests(ConfigurationFixture configurationFixture,
            TestServerFixture testServerFixture, ITestOutputHelper testOutputHelper) : base(configurationFixture,
            testServerFixture)
        {
            this.testOutputHelper = testOutputHelper;
            this.imageFilesBasePath =
                Path.Combine(AppContext.BaseDirectory, "Files");
        }

        [Fact]
        public async Task ProfileImagesController_UploadAsync_Succeeds()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (_, tokenResponse, identityUser) = await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            var imageBytes = await File.ReadAllBytesAsync(Path.Combine(this.imageFilesBasePath, "regulardoge.jpg"));

            client.SetBearerToken(tokenResponse.AccessToken);

            var uploadImageResponse = await client
                .PostAsync($"api/v1/users/{identityUser.Id}/profileimages",
                    CreateMultipartFormDataContent(imageBytes, "file", "regulardoge.jpg"));

            Assert.True(uploadImageResponse.IsSuccessStatusCode, await uploadImageResponse.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ProfileImagesController_LoadAsync_Succeeds()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (_, tokenResponse, identityUser) = await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            var imageBytes = await File.ReadAllBytesAsync(Path.Combine(this.imageFilesBasePath, "regulardoge.jpg"));

            client.SetBearerToken(tokenResponse.AccessToken);

            var uploadImageResponse = await client
                .PostAsync($"api/v1/users/{identityUser.Id}/profileimages",
                    CreateMultipartFormDataContent(imageBytes, "file", "regulardoge.jpg"));

            Assert.True(uploadImageResponse.IsSuccessStatusCode, await uploadImageResponse.Content.ReadAsStringAsync());

            identityUser = await LoadIdentityUserAssertedAsync(tokenResponse.AccessToken, client);

            var imageLoadResponse = await client.GetAsync(identityUser.ProfileImageUrl);
            Assert.True(imageLoadResponse.IsSuccessStatusCode, await imageLoadResponse.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ProfileImagesController_MultiUploadAsync_Succeeds()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (_, tokenResponse, identityUser) = await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            var imageBytes = await File.ReadAllBytesAsync(Path.Combine(this.imageFilesBasePath, "regulardoge.jpg"));

            client.SetBearerToken(tokenResponse.AccessToken);

            var uploadImageResponse = await client
                .PostAsync($"api/v1/users/{identityUser.Id}/profileimages/multiple",
                    CreateMultipartFormDataContent(imageBytes, "files", "regulardoge.jpg", 2));

            Assert.True(uploadImageResponse.IsSuccessStatusCode, await uploadImageResponse.Content.ReadAsStringAsync());

            var profileImageMetaInfos = await uploadImageResponse.Content.ReadAsAsync<ProfileImageMetaInfoDto[]>();

            Assert.Equal(2, profileImageMetaInfos.Length);
        }

        [Fact]
        public async Task ProfileImagesController_PrimaryAsync_Succeeds()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (_, tokenResponse, identityUser) = await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            var imageBytes = await File.ReadAllBytesAsync(Path.Combine(this.imageFilesBasePath, "regulardoge.jpg"));

            client.SetBearerToken(tokenResponse.AccessToken);

            var uploadImageResponse = await client
                .PostAsync($"api/v1/users/{identityUser.Id}/profileimages/multiple",
                    CreateMultipartFormDataContent(imageBytes, "files", "regulardoge.jpg", 2));


            var profileImageMetaInfos = await uploadImageResponse.Content.ReadAsAsync<ProfileImageMetaInfoDto[]>();

            var markingPrimaryImageResponse = await client.PatchAsync(
                $"api/v1/users/{identityUser.Id}/profileimages/{profileImageMetaInfos.First(img => !img.IsPrimary).Id}",
                null);

            Assert.True(markingPrimaryImageResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task ProfileImagesController_ResizeAsync_Succeeds()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (_, tokenResponse, identityUser) = await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            var imageBytes = await File.ReadAllBytesAsync(Path.Combine(this.imageFilesBasePath, "regulardoge.jpg"));

            client.SetBearerToken(tokenResponse.AccessToken);

            await client
                .PostAsync($"api/v1/users/{identityUser.Id}/profileimages",
                    CreateMultipartFormDataContent(imageBytes, "file", "regulardoge.jpg"));

            var userLoadResponse = await client.GetAsync($"api/v1/users/{identityUser.Id}");

            var userDto = await userLoadResponse.Content.ReadAsAsync<UserDto>();

            var imageLoadResizedResponse =
                await client.GetAsync($"{userDto.ProfileImageMetaInfos.First().ResizeUrl}?dimensionX=50&dimensionY=50");

            var loadedImageBytes = await imageLoadResizedResponse.Content.ReadAsByteArrayAsync();

            using (var memoryStream = new MemoryStream(loadedImageBytes))
            {
                var image = Image.FromStream(memoryStream);
                // since aspect-ratio of images are kept, the request resize-value might not be the actual resized-value
                this.testOutputHelper.WriteLine("Resized-Width: {0}, Resized-Height: {1}", image.Width, image.Height);
                Assert.True(50 >= image.Width);
                Assert.True(50 >= image.Height);
            }
        }

        [Fact]
        public async Task ProfileImagesController_RemoveAsync_Succeeds()
        {
            var client = this.TestServerFixture.ApiServer.CreateClient();

            var (_, tokenResponse, identityUser) = await this.RegisterAuthenticateAndLoadIdentityUserAsync(client);

            var imageBytes = await File.ReadAllBytesAsync(Path.Combine(this.imageFilesBasePath, "regulardoge.jpg"));

            client.SetBearerToken(tokenResponse.AccessToken);

            await client
                .PostAsync($"api/v1/users/{identityUser.Id}/profileimages",
                    CreateMultipartFormDataContent(imageBytes, "file", "regulardoge.jpg"));

            var userLoadResponse = await client.GetAsync($"api/v1/users/{identityUser.Id}");

            var userDto = await userLoadResponse.Content.ReadAsAsync<UserDto>();

            var imageRemovelResponse =
                await client.DeleteAsync(
                    $"api/v1/users/{identityUser.Id}/profileimages/{userDto.ProfileImageMetaInfos.First().Id}");

            Assert.True(imageRemovelResponse.IsSuccessStatusCode,
                await imageRemovelResponse.Content.ReadAsStringAsync());
        }

        private async Task<(UserRegisterDto, TokenResponse, IdentityUserDto)>
            RegisterAuthenticateAndLoadIdentityUserAsync(HttpClient client)
        {
            var registerDto = await RegisterAssertedAsync(client);

            var accessTokenDto = await this.GetTokenAsync(registerDto);

            var identityUserDto = await LoadIdentityUserAssertedAsync(accessTokenDto.AccessToken, client);

            return (registerDto, accessTokenDto, identityUserDto);
        }

        private static HttpContent CreateMultipartFormDataContent(byte[] fileBytes, string key, string name,
            int quantity = 1)
        {
            var byteArrayContents = Enumerable.Range(0, quantity)
                .Select(_ =>
                {
                    var byteArrayContent = new ByteArrayContent(fileBytes);
                    byteArrayContent.Headers.Add("Content-Type", "image/jpg");
                    return byteArrayContent;
                })
                .ToArray();

            var multiPartFormDataContent = new MultipartFormDataContent();

            foreach (var byteArrayContent in byteArrayContents)
            {
                multiPartFormDataContent.Add(byteArrayContent, key, name);
            }


            return multiPartFormDataContent;
        }
    }
}