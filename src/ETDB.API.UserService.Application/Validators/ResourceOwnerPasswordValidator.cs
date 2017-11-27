using System.Threading.Tasks;
using ETDB.API.ServiceBase.General.Abstractions.Hasher;
using ETDB.API.UserService.Repositories.Abstractions;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace ETDB.API.UserService.Application.Validators
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

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var loginUser = this.userRepository.Find(context.UserName, context.UserName);

            if (loginUser == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

                return Task.FromResult(context.Result);
            }

            if (loginUser.Password != this.hasher.CreateSaltedHash(context.Password, loginUser.Salt))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

                return Task.FromResult(context.Result);
            }

            context.Result = new GrantValidationResult(loginUser.Id.ToString(),
                "custom", this.userRepository.GetClaims(loginUser));

            return Task.FromResult(context.Result);
        }
    }
}
