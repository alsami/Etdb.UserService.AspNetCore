using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Users;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Etdb.UserService.Authentication.Strategies
{
    public abstract class ExternalAuthenticationStrategyBase
    {
        private readonly IBus bus;
        protected readonly IExternalIdentityServerClient ExternalIdentityServerClient;

        protected ExternalAuthenticationStrategyBase(IBus bus,
            IExternalIdentityServerClient externalIdentityServerClient)
        {
            this.bus = bus;
            this.ExternalIdentityServerClient = externalIdentityServerClient;
        }

        protected abstract string UserProfileUrl { get; }

        protected abstract AuthenticationProvider AuthenticationProvider { get; }

        protected virtual JsonSerializerSettings SerializeSettings => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        protected virtual GrantValidationResult NotEqualSignInProviderResult
            => new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                "Email address already registered for a different provider!");

        protected bool AreSignInProvidersEqual(UserDto userDto)
        {
            var userSignInProvider =
                (AuthenticationProvider) Enum.Parse(typeof(AuthenticationProvider), userDto.SignInProvider);

            return userSignInProvider == this.AuthenticationProvider;
        }

        protected async Task<UserDto> SearchForExistingUserAsync(string emailAddress)
        {
            var userSearchCommand = new UserSearchByUsernameAndEmailCommand(emailAddress);

            return await this.bus.SendCommandAsync<UserSearchByUsernameAndEmailCommand, UserDto>(userSearchCommand);
        }

        protected async Task<GrantValidationResult> SuccessValidationResultAsync(UserDto user)
        {
            var claims =
                await this.bus.SendCommandAsync<UserClaimsLoadCommand, IEnumerable<Claim>>(
                    new UserClaimsLoadCommand(user.Id));

            return new GrantValidationResult(user.Id.ToString(),
                Misc.Constants.Identity.ExternalGrantType, claims,
                this.AuthenticationProvider.ToString());
        }

        protected async Task<UserDto> RegisterUserAsync(UserRegisterCommand command)
        {
            var user = await this.bus.SendCommandAsync<UserRegisterCommand, UserDto>(command);

            return user;
        }
    }
}