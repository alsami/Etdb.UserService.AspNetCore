using System.Threading.Tasks;
using Etdb.UserService.Services.Abstractions;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Etdb.UserService.Application.Validators
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserService userService;

        public ResourceOwnerPasswordValidator(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var loginUser = await this.userService.FindUserByUserNameOrEmailAsync(context.UserName);

            if (loginUser == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            if (this.userService.ArePasswordsEqual(loginUser, context.Password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            context.Result = new GrantValidationResult(loginUser.Id.ToString(), 
                "custom", 
                await this.userService.AllocateClaims(loginUser));
        }
    }
}
