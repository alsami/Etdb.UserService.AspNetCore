using System.Threading.Tasks;
using Etdb.ServiceBase.General.Abstractions.Hasher;
using Etdb.UserService.Repositories.Abstractions;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Etdb.UserService.Application.Validators
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository userRepository;
        private readonly IHasher hasher;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository, IHasher hasher)
        {
            this.userRepository = userRepository;
            this.hasher = hasher;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var loginUser = await this.userRepository.FindAsync(context.UserName, context.UserName);

            if (loginUser == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            if (loginUser.Password != this.hasher.CreateSaltedHash(context.Password, loginUser.Salt))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            context.Result = new GrantValidationResult(loginUser.Id, "custom", await this.userRepository.GetClaims(loginUser));
        }
    }
}
