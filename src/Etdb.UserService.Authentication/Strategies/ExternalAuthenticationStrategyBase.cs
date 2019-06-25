using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Abstractions.Events.Authentication;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Users;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Etdb.UserService.Authentication.Strategies
{
    public abstract class ExternalAuthenticationStrategyBase
    {
        private readonly IBus bus;
        private readonly IHttpContextAccessor httpContextAccessor;
        protected readonly IExternalIdentityServerClient ExternalIdentityServerClient;

        protected ExternalAuthenticationStrategyBase(IBus bus,
            IExternalIdentityServerClient externalIdentityServerClient, IHttpContextAccessor httpContextAccessor)
        {
            this.bus = bus;
            this.ExternalIdentityServerClient = externalIdentityServerClient;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected abstract AuthenticationProvider AuthenticationProvider { get; }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
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
                (AuthenticationProvider) Enum.Parse(typeof(AuthenticationProvider), userDto.AuthenticationProvider);

            return userSignInProvider == this.AuthenticationProvider;
        }

        protected async Task<UserDto> SearchForExistingUserAsync(string emailAddress)
        {
            var userSearchCommand = new UserSearchByUsernameAndEmailCommand(emailAddress);

            return await this.bus.SendCommandAsync<UserSearchByUsernameAndEmailCommand, UserDto>(userSearchCommand);
        }

        protected async Task<GrantValidationResult> SuccessValidationResultAsync(UserDto user)
        {
            await this.PublishAuthenticationEvent(this.CreateUserAuthenticatedEvent(user, AuthenticationLogType.Succeeded, $"Authenticated using a {this.AuthenticationProvider} token"));
            
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

        protected Task PublishAuthenticationEvent(UserAuthenticatedEvent @event)
            => this.bus.RaiseEventAsync(@event);
        
        protected UserAuthenticatedEvent CreateUserAuthenticatedEvent(UserDto user, AuthenticationLogType authenticationLogType, string additionalInfo = null)
            => new UserAuthenticatedEvent(authenticationLogType.ToString(),
                this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress, user.Id,
                DateTime.UtcNow, additionalInfo);
    }
}