using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Presentation.Enums;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;

namespace Etdb.UserService.Authentication.Validator
{
    public class ResourceOwnerPasswordGrantValidator : IResourceOwnerPasswordValidator
    {
        private const string InvalidUserOrPasswordError = "Invalid user or password";
        private const string UserLockedOutError = "User locked out";

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBus bus;

        public ResourceOwnerPasswordGrantValidator(IHttpContextAccessor httpContextAccessor, IBus bus)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.bus = bus;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var command = new UserSignInValidationCommand(context.UserName, context.Password,
                this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

            var userLoginValidation =
                await this.bus.SendCommandAsync<UserSignInValidationCommand, SignInValidationDto>(command);

            if (userLoginValidation.IsValid)
            {
                var claims =
                    await this.bus.SendCommandAsync<UserClaimsLoadCommand, IEnumerable<Claim>>(
                        new UserClaimsLoadCommand(userLoginValidation.UserId));

                context.Result = new GrantValidationResult(userLoginValidation.UserId.ToString(),
                    Misc.Constants.Identity.PasswordAuthenticationMethod,
                    claims, AuthenticationProvider.UsernamePassword.ToString());

                return;
            }

            if (userLoginValidation.signInFailure == SignInFailure.Unavailable ||
                userLoginValidation.signInFailure == SignInFailure.InvalidPassword)
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