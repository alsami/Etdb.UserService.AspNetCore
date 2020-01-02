using System;
using System.Net;
using System.Threading.Tasks;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Abstractions.Events.Users;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Users;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Etdb.UserService.Authentication.Strategies
{
    public abstract class ExternalAuthenticationStrategyBase
    {
        private readonly IMediator bus;
        protected readonly IExternalIdentityServerClient ExternalIdentityServerClient;

        protected ExternalAuthenticationStrategyBase(IMediator bus,
            IExternalIdentityServerClient externalIdentityServerClient)
        {
            this.bus = bus;
            this.ExternalIdentityServerClient = externalIdentityServerClient;
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

        protected async Task<UserDto?> SearchForExistingUserAsync(string emailAddress)
        {
            var userSearchCommand = new UserSearchByUsernameAndEmailCommand(emailAddress);

            return await this.bus.Send(userSearchCommand);
        }

        protected async Task<GrantValidationResult> SuccessValidationResultAsync(UserDto user, IPAddress ipAddress)
        {
            await this.PublishAuthenticationEvent(this.CreateUserAuthenticatedEvent(user,
                AuthenticationLogType.Succeeded, ipAddress, $"Authenticated using a {this.AuthenticationProvider} token"));

            var claims =
                await this.bus.Send(
                    new ClaimsLoadCommand(user.Id));

            return new GrantValidationResult(user.Id.ToString(),
                Misc.Constants.Identity.ExternalGrantType, claims,
                this.AuthenticationProvider.ToString());
        }

        protected async Task<UserDto> RegisterUserAsync(UserRegisterCommand command)
        {
            var user = await this.bus.Send(command);

            return user!;
        }

        protected Task PublishAuthenticationEvent(UserAuthenticatedEvent @event)
            => this.bus.Publish(@event);

        protected UserAuthenticatedEvent CreateUserAuthenticatedEvent(UserDto user,
            AuthenticationLogType authenticationLogType, IPAddress ipAddress, string? additionalInfo = null)
            => new UserAuthenticatedEvent(user.Id, user.UserName, authenticationLogType.ToString(),
                ipAddress, DateTime.UtcNow, additionalInfo);
    }
}