using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Authentication.Structures;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Enums;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Etdb.UserService.Authentication.Strategies
{
    public class FacebookAuthenticationStrategy : ExternalAuthenticationStrategyBase, IFacebookAuthenticationStrategy
    {
        private readonly ILogger<FacebookAuthenticationStrategy> logger;
        private readonly IBus bus;
        private readonly IHttpClientFactory httpClientFactory;

        public FacebookAuthenticationStrategy(IHttpClientFactory httpClientFactory,
            ILogger<FacebookAuthenticationStrategy> logger, IBus bus) : base(bus)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.bus = bus;
        }

        protected override string UserProfileUrl => "https://graph.facebook.com/v3.2/me";
        protected override SignInProvider SignInProvider => SignInProvider.Facebook;

        public async Task<GrantValidationResult> AuthenticateAsync(string token)
        {
            var url = new StringBuilder(this.UserProfileUrl)
                .Append($"?access_token={token}")
                .Append("&fields=id,email,name,gender,birthday,picture")
                .ToString();

            var client = this.httpClientFactory.CreateClient();

            var response = await client.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return ErrorValidationResult(json);
            }

            var facebookUser = JsonConvert.DeserializeObject<FacebookUserProfile>(json, this.SerializeSettings);

            var existingUser = await this.SearchForExistingUserAsync(facebookUser.Email);

            if (existingUser != null)
            {
                if (this.AreSignInProvidersEqual(existingUser))
                {
                    return await this.SuccessValidationResultAsync(existingUser);
                }

                return this.NotEqualSignInProviderResult;
            }

            var command = await this.CreateCommandAsync(client, facebookUser);

            var registeredUser = await this.RegisterUserAsync(command);

            return await this.SuccessValidationResultAsync(registeredUser);
        }

        private async Task<UserRegisterCommand> CreateCommandAsync(HttpClient client, FacebookUserProfile facebookUser)
        {
            var profileImageAddCommand = !string.IsNullOrWhiteSpace(facebookUser.Picture?.Data?.Url)
                ? new UserProfileImageAddCommand("facebook_photo.jpg", new ContentType("image/*"),
                    await client.GetByteArrayAsync(facebookUser.Picture.Data.Url))
                : null;

            var firstIndexOfWhitespace = facebookUser.Name.IndexOf(" ", StringComparison.Ordinal);

            var firstName = facebookUser.Name.Substring(0, firstIndexOfWhitespace - 1);
            var lastName = facebookUser.Name.Substring(firstIndexOfWhitespace + 1, facebookUser.Name.Length - 1 - firstIndexOfWhitespace);

            return new UserRegisterCommand(facebookUser.Email, firstName, lastName, new List<EmailAddCommand>()
            {
                new EmailAddCommand(facebookUser.Email, true, true)
            }, (int) this.SignInProvider, profileImageAddCommand: profileImageAddCommand);
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