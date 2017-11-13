using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace EntertainmentDatabase.REST.API.AuthService.Bootstrap.Validators
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.UserName == "test123" && context.Password == "password")
            {
                context.Result = new GrantValidationResult(subject: "81822", authenticationMethod: "custom");
                return Task.FromResult(context.Result);
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            return Task.FromResult(context.Result);
        }
    }
}
