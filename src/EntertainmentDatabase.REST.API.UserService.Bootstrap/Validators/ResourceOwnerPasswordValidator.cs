using System;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.API.ServiceBase.Common.Base;
using EntertainmentDatabase.REST.API.ServiceBase.Common.Factory;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.UserService.Domain.Entities;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace EntertainmentDatabase.REST.API.UserService.Bootstrap.Validators
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
                .Get(user => user.Email.Equals(context.UserName, StringComparison.OrdinalIgnoreCase)
                             || user.UserName.Equals(context.UserName, StringComparison.OrdinalIgnoreCase));

            if (loginUser == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

                return Task.FromResult(context.Result);
            }

            if (loginUser.Password == this.hasher.CreateSaltedHash(context.Password, loginUser.Salt))
            {
                context.Result = new GrantValidationResult(subject: loginUser.Id.ToString(), 
                    authenticationMethod: "custom", claims:);
                return Task.FromResult(context.Result);
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            return Task.FromResult(context.Result);
        }
    }
}
