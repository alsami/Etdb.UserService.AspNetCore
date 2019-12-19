using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Authentication.Structures;
using Etdb.UserService.Cqrs.Abstractions.Commands.Emails;
using Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Enums;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Etdb.UserService.Authentication.Strategies
{
    public class GoogleAuthenticationStrategy : ExternalAuthenticationStrategyBase, IGoogleAuthenticationStrategy
    {
        private readonly ILogger<GoogleAuthenticationStrategy> logger;

        private static string UserProfileUrl => "https://www.googleapis.com/oauth2/v2/userinfo";

        public GoogleAuthenticationStrategy(IMediator bus, IExternalIdentityServerClient externalIdentityServerClient,
            IHttpContextAccessor httpContextAccessor, ILogger<GoogleAuthenticationStrategy> logger) : base(bus,
            externalIdentityServerClient, httpContextAccessor)
        {
            this.logger = logger;
        }

        protected override AuthenticationProvider AuthenticationProvider => AuthenticationProvider.Google;

        public async Task<GrantValidationResult> AuthenticateAsync(string token)
        {
            var response =
                await this.ExternalIdentityServerClient.Client.GetAsync($"{UserProfileUrl}?access_token={token}");

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return this.ErrorValidationResult(json);

            var googleProfile =
                JsonConvert.DeserializeObject<GoogleUserProfile>(json, this.SerializeSettings);

            var existingUser = await this.SearchForExistingUserAsync(googleProfile!.Email);

            if (existingUser != null)
            {
                if (this.AreSignInProvidersEqual(existingUser))
                    return await this.SuccessValidationResultAsync(existingUser);

                await this.PublishAuthenticationEvent(this.CreateUserAuthenticatedEvent(existingUser,
                    AuthenticationLogType.Failed,
                    $"User is already registered using provider {existingUser.AuthenticationProvider}"));

                return this.NotEqualSignInProviderResult;
            }

            var command = await CreateCommandAsync(this.ExternalIdentityServerClient.Client, googleProfile);

            var registeredUser = await this.RegisterUserAsync(command);

            return await this.SuccessValidationResultAsync(registeredUser);
        }

        private static async Task<UserRegisterCommand> CreateCommandAsync(HttpClient client,
            GoogleUserProfile googleProfile)
        {
            var userId = Guid.NewGuid();

            var emailAddCommand = new EmailAddCommand(Guid.NewGuid(), googleProfile.Email, true, true);

            var profileImageAddCommand = new ProfileImageAddCommand(userId,
                "google_photo.jpg",
                new ContentType("image/*"), await client.GetByteArrayAsync(googleProfile.Picture));

            return new UserRegisterCommand(userId, googleProfile.Email, googleProfile.Given_Name,
                googleProfile.Family_Name, new[]
                {
                    emailAddCommand
                }, (int) AuthenticationProvider.Google, profileImageAddCommand: profileImageAddCommand);
        }


        private GrantValidationResult ErrorValidationResult(string json)
        {
            var errorContainer =
                JsonConvert.DeserializeObject<StandardizedAuthErrorContainer>(json,
                    this.SerializeSettings);

            this.logger.LogError(
                $"Google authentication failed with code {errorContainer!.Error.Code} and message\n{errorContainer.Error.Message}!");

            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, errorContainer.Error.Message);
        }
    }
}