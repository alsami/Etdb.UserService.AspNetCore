using System;
using System.Linq;
using System.Threading.Tasks;
using ETDB.API.ServiceBase.Common.Base;
using ETDB.API.ServiceBase.Common.Factory;
using ETDB.API.ServiceBase.Generics.Base;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Repositories.Base;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace ETDB.API.UserService.Bootstrap.Validators
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IEntityRepository<User> userRepository;
        private readonly IUserClaimsRepository userClaimsRepository;
        private readonly IHashingStrategy hasher;

        public ResourceOwnerPasswordValidator(IEntityRepository<User> userRepository, IUserClaimsRepository userClaimsRepository)
        {
            this.userRepository = userRepository;
            this.userClaimsRepository = userClaimsRepository;
            this.hasher = new HasherFactory()
                .CreateHasher(KeyDerivationPrf.HMACSHA1);
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var loginUser = this.userRepository
                .Get(user => user.Email.Equals(context.UserName, StringComparison.OrdinalIgnoreCase)
                             || user.UserName.Equals(context.UserName, StringComparison.OrdinalIgnoreCase));

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
                "custom", this.userClaimsRepository.GetClaims(loginUser));

            return Task.FromResult(context.Result);
        }
    }
}
