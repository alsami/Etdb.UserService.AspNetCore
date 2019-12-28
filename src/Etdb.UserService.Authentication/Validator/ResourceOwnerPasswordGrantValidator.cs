using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Presentation.Enums;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Etdb.UserService.Authentication.Validator
{
    public class ResourceOwnerPasswordGrantValidator : IResourceOwnerPasswordValidator
    {
        private const string InvalidUserOrPasswordError = "Invalid user or password";
        private const string UserLockedOutError = "User locked out";

        private readonly IMediator bus;

        public ResourceOwnerPasswordGrantValidator(IMediator bus)
        {
            this.bus = bus;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var command = new UserAuthenticationValidationCommand(context.UserName, context.Password,
                IPAddress.Parse(context.Request.Raw.Get("IpAddress") ?? "127.0.0.1"));

            var userAuthenticationValidation =
                await this.bus.Send(
                    command);

            if (userAuthenticationValidation!.IsValid)
            {
                var claims =
                    await this.bus.Send(
                        new ClaimsLoadCommand(userAuthenticationValidation.UserId));

                context.Result = new GrantValidationResult(userAuthenticationValidation.UserId.ToString(),
                    Misc.Constants.Identity.PasswordAuthenticationMethod,
                    claims, AuthenticationProvider.UsernamePassword.ToString());

                return;
            }

            if (userAuthenticationValidation.AuthenticationFailure == AuthenticationFailure.Unavailable ||
                userAuthenticationValidation.AuthenticationFailure == AuthenticationFailure.InvalidPassword)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    ResourceOwnerPasswordGrantValidator.InvalidUserOrPasswordError);

                return;
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                ResourceOwnerPasswordGrantValidator.UserLockedOutError);
        }
    }
}