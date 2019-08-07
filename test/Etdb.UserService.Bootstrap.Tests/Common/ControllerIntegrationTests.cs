using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Etdb.UserService.Bootstrap.Tests.Fixtures;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Presentation.Users;
using IdentityModel.Client;
using IdentityServer4.Contrib.AspNetCore.Testing.Configuration;
using IdentityServer4.Contrib.AspNetCore.Testing.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Xunit;

namespace Etdb.UserService.Bootstrap.Tests.Common
{
    public abstract class ControllerIntegrationTests : IClassFixture<ConfigurationFixture>,
        IClassFixture<TestServerFixture>
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected readonly ConfigurationFixture ConfigurationFixture;
        protected readonly TestServerFixture TestServerFixture;

        protected ControllerIntegrationTests(ConfigurationFixture configurationFixture,
            TestServerFixture testServerFixture)
        {
            this.ConfigurationFixture = configurationFixture;
            this.TestServerFixture = testServerFixture;
        }

        protected string GetClientId() => this.ConfigurationFixture
            .Configuration
            .GetSection(nameof(IdentityServerConfiguration))
            .Get<IdentityServerConfiguration>()
            .Clients
            .First()
            .Id;

        protected async Task<TokenResponse> GetTokenAsync(UserRegisterDto registerDto)
        {
            var proxy = new IdentityServerProxy(this.TestServerFixture.IdentityServer);

            var tokenResponse = await proxy.GetResourceOwnerPasswordAccessTokenAsync(
                new ClientConfiguration(this.GetClientId(), this.GetClientSecret()),
                new UserLoginConfiguration(registerDto.UserName, registerDto.Password),
                string.Join(" ", this.GetClientScopes()));

            return tokenResponse;
        }

        protected static async Task<UserRegisterDto> RegisterAssertedAsync(HttpClient httpClient)
        {
            var registerDto = CreateRandomRegistrationDto();

            var response = await RegisterAsync(registerDto, httpClient);

            Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());

            return registerDto;
        }

        protected static UserRegisterDto CreateRandomRegistrationDto()
            => new UserRegisterDto("etdb", "etdb",
                Guid.NewGuid().ToString().Replace("-", ""), "supersecret-password",
                new List<AddEmailDto>
                {
                    new AddEmailDto($"{DateTime.UtcNow.Ticks}etdb{DateTime.UtcNow.Ticks}@etbd.com", true)
                });

        protected static Task<HttpResponseMessage> RegisterAsync(UserRegisterDto registerDto, HttpClient client)
            => client.PostAsJsonAsync("/api/v1/auth/registration", registerDto);

        protected static Task<HttpResponseMessage> LoadIdentityUserAsync(string accessToken, HttpClient client)
        {
            client.SetBearerToken(accessToken);
            return client.GetAsync($"api/v1/auth/user-identity/{accessToken}");
        }

        protected static async Task<IdentityUserDto> LoadIdentityUserAssertedAsync(string accessToken,
            HttpClient client)
        {
            var response = await LoadIdentityUserAsync(accessToken, client);

            Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());

            return await response.Content.ReadAsAsync<IdentityUserDto>();
        }

        protected Task<HttpResponseMessage> RefreshAuthenticationAsync(string refreshToken, HttpClient client,
            AuthenticationProvider authenticationProvider)
            => client.GetAsync(
                $"api/v1/auth/refresh-authentication/{refreshToken}/{this.GetClientId()}/{authenticationProvider.ToString()}");

        protected static Task<HttpResponseMessage> AuthenticateAsync(
            InternalAuthenticationDto internalAuthenticationDto, HttpClient client)
            => client.PostAsJsonAsync("api/v1/auth/authentication", internalAuthenticationDto);


        private string GetClientSecret() => this.ConfigurationFixture
            .Configuration
            .GetSection(nameof(IdentityServerConfiguration))
            .Get<IdentityServerConfiguration>()
            .Clients
            .First()
            .Secret;

        private string[] GetClientScopes() => this.ConfigurationFixture
            .Configuration
            .GetSection(nameof(IdentityServerConfiguration))
            .Get<IdentityServerConfiguration>()
            .Clients
            .First()
            .Scopes;
    }
}