using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Authentication.Structures;
using Etdb.UserService.Constants;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Etdb.UserService.Authentication.Strategies
{
    public class GoogleAuthenticationStrategy : ExternalAuthenticationStrategyBase, IGoogleAuthenticationStrategy
    {
        private readonly ILogger<GoogleAuthenticationStrategy> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IBus bus;

        public GoogleAuthenticationStrategy(IHttpClientFactory httpClientFactory,
            ILogger<GoogleAuthenticationStrategy> logger, IBus bus) : base(bus)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.bus = bus;
        }

        protected override string UserProfileUrl => "https://www.googleapis.com/oauth2/v2/userinfo";
        protected override SignInProvider SignInProvider => SignInProvider.Google;

        public async Task<GrantValidationResult> AuthenticateAsync(string token)
        {
            var client = this.httpClientFactory.CreateClient();

            var response = await client.GetAsync($"{this.UserProfileUrl}?access_token={token}");

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return this.ErrorValidationResult(json);
            }

            var googleProfile =
                JsonConvert.DeserializeObject<GoogleUserProfile>(json, this.SerializeSettings);

            var existingUser = await this.SearchForExistingUserAsync(googleProfile.Email);

            if (existingUser != null)
            {
                if (this.AreSignInProvidersEqual(existingUser))
                {
                    return await this.SuccessValidationResultAsync(existingUser);
                }

                return this.NotEqualSignInProviderResult;
            }

            var command = await CreateCommandAsync(client, googleProfile);

            var registeredUser = await this.RegisterUserAsync(command);

            return await this.SuccessValidationResultAsync(registeredUser);
        }

        private static async Task<UserRegisterCommand> CreateCommandAsync(HttpClient client,
            GoogleUserProfile googleProfile)
            => new UserRegisterCommand(googleProfile.Email, googleProfile.Given_Name,
                googleProfile.Family_Name, new[]
                {
                    new EmailAddCommand(googleProfile.Email, true, true),
                }, (int) SignInProvider.Google, profileImageAddCommand: new UserProfileImageAddCommand(
                    "google_photo.jpg",
                    new ContentType("image/*"), await client.GetByteArrayAsync(googleProfile.Picture)));


        private GrantValidationResult ErrorValidationResult(string json)
        {
            var errorContainer =
                JsonConvert.DeserializeObject<StandardizedAuthErrorContainer>(json,
                    this.SerializeSettings);

            this.logger.LogError(
                $"Google authentication failed with code {errorContainer.Error.Code} and message\n{errorContainer.Error.Message}!");

            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, errorContainer.Error.Message);
        }
    }
}