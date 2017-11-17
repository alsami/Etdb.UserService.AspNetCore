using System;
using System.Linq;
using System.Threading.Tasks;
using ETDB.API.ServiceBase.Common.Base;
using ETDB.API.ServiceBase.Common.Factory;
using ETDB.API.ServiceBase.Generics.Base;
using ETDB.API.UserService.Bootstrap.Extensions;
using ETDB.API.UserService.Domain.Entities;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace ETDB.API.UserService.Bootstrap.Validators
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IEntityRepository<User> userRepository;
        private readonly IHashingStrategy hasher;

        public ResourceOwnerPasswordValidator(IEntityRepository<User> userRepository)
        {
            this.userRepository = userRepository;
            this.hasher = new HasherFactory()
                .CreateHasher(KeyDerivationPrf.HMACSHA1);
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var loginUser = this.userRepository
                .GetQueryable()
                .Include(user => user.UserSecurityroles)
                .ThenInclude(userSercurityrole => userSercurityrole.Securityrole)
                .FirstOrDefault(user => user.Email.Equals(context.UserName, StringComparison.OrdinalIgnoreCase) 
                    || user.UserName.Equals(context.UserName, StringComparison.OrdinalIgnoreCase));

            if (loginUser == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

                return Task.FromResult(context.Result);
            }

            if (loginUser.Password == this.hasher.CreateSaltedHash(context.Password, loginUser.Salt))
            {
                context.Result = new GrantValidationResult(loginUser.Id.ToString(), 
                    "custom", loginUser.GetClaims());
                return Task.FromResult(context.Result);
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            return Task.FromResult(context.Result);
        }
    }
}
