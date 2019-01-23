using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Constants;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Etdb.UserService.Authentication.Strategies
{
    public abstract class ExternalAuthenticationStrategyBase
    {
        private readonly IBus bus;

        protected ExternalAuthenticationStrategyBase(IBus bus)
        {
            this.bus = bus;
        }

        protected abstract string UserProfileUrl { get; }

        protected abstract SignInProvider SignInProvider { get; }

        protected virtual JsonSerializerSettings SerializeSettings => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        protected virtual GrantValidationResult NotEqualSignInProviderResult
            => new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                "Email address already registered for a different provider!");

        protected bool AreSignInProvidersEqual(UserDto userDto)
        {
            var userSignInProvider = (SignInProvider) Enum.Parse(typeof(SignInProvider), userDto.SignInProvider);

            return userSignInProvider == this.SignInProvider;
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
                Misc.ExternalGrantType, claims,
                this.SignInProvider.ToString());
        }

        protected async Task<UserDto> RegisterUserAsync(UserRegisterCommand command)
        {
            var user = await this.bus.SendCommandAsync<UserRegisterCommand, UserDto>(command);

            return user;
        }
    }
}