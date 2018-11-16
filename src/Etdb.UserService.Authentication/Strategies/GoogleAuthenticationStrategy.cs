using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Authentication.Abstractions;
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

        private static readonly JsonSerializerSettings SerializeSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public GoogleAuthenticationStrategy(IHttpClientFactory httpClientFactory,
            ILogger<GoogleAuthenticationStrategy> logger, IBus bus)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.bus = bus;
        }

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
                JsonConvert.DeserializeObject<GoogleUserProfile>(json, GoogleAuthenticationStrategy.SerializeSettings);

            var existingUser = await this.SearchForExistingUserAsync(googleProfile);

            if (existingUser != null)
            {
                return await this.SuccessValidationResultAsync(existingUser);
            }

            var newUser = await this.RegisterUserFromAuthResponseAsync(googleProfile);

            await this.AddUserProfileImageAsync(newUser.Id, await client.GetByteArrayAsync(googleProfile.Picture));

            return await this.SuccessValidationResultAsync(newUser);
        }

        protected override string UserProfileUrl => "https://www.googleapis.com/oauth2/v2/userinfo";

        private async Task<UserDto> SearchForExistingUserAsync(GoogleUserProfile googleProfile)
        {
            var userSearchCommand = new UserSearchByUsernameAndEmailCommand(googleProfile.Email);

            return await this.bus.SendCommandAsync<UserSearchByUsernameAndEmailCommand, UserDto>(userSearchCommand);
        }

        private async Task AddUserProfileImageAsync(Guid id, byte[] userImageFileBytes)
        {
            var profileImageAddCommand = new UserProfileImageAddCommand(id, "google_photo.jpg",
                new ContentType("image/*"), userImageFileBytes);

            await this.bus.SendCommandAsync<UserProfileImageAddCommand, UserDto>(profileImageAddCommand);
        }

        private async Task<UserDto> RegisterUserFromAuthResponseAsync(GoogleUserProfile googleProfile)
        {
            var registerCommand = new UserRegisterCommand(googleProfile.Email, googleProfile.Given_Name,
                googleProfile.Family_Name, new[]
                {
                    new EmailAddCommand(googleProfile.Email, true, true),
                }, (int) SignInProvider.Google);

            var user = await this.bus.SendCommandAsync<UserRegisterCommand, UserDto>(registerCommand);

            return user;
        }
        
        private async Task<GrantValidationResult> SuccessValidationResultAsync(UserDto user)
        {
            var claims =
                await this.bus.SendCommandAsync<UserClaimsLoadCommand, IEnumerable<Claim>>(
                    new UserClaimsLoadCommand(user.Id));

            return new GrantValidationResult(user.Id.ToString(),
                Misc.ExternalGrantType, claims,
                SignInProvider.Google.ToString());
        }

        private GrantValidationResult ErrorValidationResult(string json)
        {
            var errorContainer =
                JsonConvert.DeserializeObject<GoogleAuthErrorContainer>(json,
                    GoogleAuthenticationStrategy.SerializeSettings);

            this.logger.LogError(
                $"Google authentication failed with code {errorContainer.Error.Code} and message\n{errorContainer.Error.Message}!");

            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, errorContainer.Error.Message);
        }
    }
}