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
using Microsoft.AspNetCore.Http;

namespace Etdb.UserService.Authentication.Validator
{
    public class ResourceOwnerPasswordGrandValidator : IResourceOwnerPasswordValidator
    {
        private const string InvalidUserOrPasswordError = "Invalid user or password";

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBus bus;

        public ResourceOwnerPasswordGrandValidator(IHttpContextAccessor httpContextAccessor, IBus bus)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.bus = bus;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var command = new UserLoginValidationCommand(context.UserName, context.Password,
                this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

            var userLoginValidation =
                await this.bus.SendCommandAsync<UserLoginValidationCommand, UserLoginValidationDto>(command);

            if (!userLoginValidation.IsValid)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, ResourceOwnerPasswordGrandValidator.InvalidUserOrPasswordError);
                return;
            }

            var claims =
                await this.bus.SendCommandAsync<UserClaimsLoadCommand, IEnumerable<Claim>>(
                    new UserClaimsLoadCommand(userLoginValidation.UserId));

            context.Result = new GrantValidationResult(userLoginValidation.UserId.ToString(),
                Misc.CustomAuthenticationMethod,
                claims, SignInProvider.UsernamePassword.ToString());
        }
    }
}