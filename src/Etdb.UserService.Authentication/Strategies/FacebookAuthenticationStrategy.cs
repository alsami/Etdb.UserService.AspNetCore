using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Authentication.Structures;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Abstractions.Events.Authentication;
using Etdb.UserService.Domain.Enums;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Etdb.UserService.Authentication.Strategies
{
    public class FacebookAuthenticationStrategy : ExternalAuthenticationStrategyBase, IFacebookAuthenticationStrategy
    {
        private readonly ILogger<FacebookAuthenticationStrategy> logger;


        private static string UserProfileUrl => "https://graph.facebook.com/v3.2/me";
        protected override AuthenticationProvider AuthenticationProvider => AuthenticationProvider.Facebook;

        public FacebookAuthenticationStrategy(IBus bus, IExternalIdentityServerClient externalIdentityServerClient,
            IHttpContextAccessor httpContextAccessor, ILogger<FacebookAuthenticationStrategy> logger) : base(bus,
            externalIdentityServerClient, httpContextAccessor)
        {
            this.logger = logger;
        }

        public async Task<GrantValidationResult> AuthenticateAsync(string token)
        {
            var url = new StringBuilder(UserProfileUrl)
                .Append($"?access_token={token}")
                .Append("&fields=id,email,name,gender,birthday,picture")
                .ToString();

            var response = await this.ExternalIdentityServerClient.Client.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return this.ErrorValidationResult(json);
            }

            var facebookUser = JsonConvert.DeserializeObject<FacebookUserProfile>(json, this.SerializeSettings);

            var existingUser = await this.SearchForExistingUserAsync(facebookUser.Email);

            if (existingUser != null)
            {
                if (this.AreSignInProvidersEqual(existingUser))
                    return await this.SuccessValidationResultAsync(existingUser);

                await this.PublishAuthenticationEvent(this.CreateUserAuthenticatedEvent(existingUser,
                    AuthenticationLogType.Failed,
                    $"User is already registered using provider {existingUser.AuthenticationProvider}"));

                return this.NotEqualSignInProviderResult;
            }

            var command = await this.CreateCommandAsync(this.ExternalIdentityServerClient.Client, facebookUser);

            var registeredUser = await this.RegisterUserAsync(command);

            return await this.SuccessValidationResultAsync(registeredUser);
        }

        private async Task<UserRegisterCommand> CreateCommandAsync(HttpClient client, FacebookUserProfile facebookUser)
        {
            var userId = Guid.NewGuid();

            var profileImageAddCommand = !string.IsNullOrWhiteSpace(facebookUser.Picture?.Data?.Url)
                ? new ProfileImageAddCommand(userId, "facebook_photo.jpg", new ContentType("image/*"),
                    await client.GetByteArrayAsync(facebookUser.Picture.Data.Url))
                : null;

            var firstIndexOfWhitespace = facebookUser.Name.IndexOf(" ", StringComparison.Ordinal);

            var firstName = facebookUser.Name.Substring(0, firstIndexOfWhitespace - 1);
            var lastName = facebookUser.Name.Substring(firstIndexOfWhitespace + 1,
                facebookUser.Name.Length - 1 - firstIndexOfWhitespace);

            return new UserRegisterCommand(userId, facebookUser.Email, firstName, lastName,
                new List<EmailAddCommand>()
                {
                    new EmailAddCommand(Guid.NewGuid(), facebookUser.Email, true, true)
                }, (int) this.AuthenticationProvider, profileImageAddCommand: profileImageAddCommand);
        }

        private GrantValidationResult ErrorValidationResult(string json)
        {
            var errorContainer =
                JsonConvert.DeserializeObject<StandardizedAuthErrorContainer>(json,
                    this.SerializeSettings);

            this.logger.LogError(
                $"Facebook authentication failed with code {errorContainer.Error.Code} and message\n{errorContainer.Error.Message}!");

            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, errorContainer.Error.Message);
        }
    }
}