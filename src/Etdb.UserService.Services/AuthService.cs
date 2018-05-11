using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Domain;
using Etdb.UserService.Extensions;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Caching.Distributed;

namespace Etdb.UserService.Services
{
    public class AuthService : IAuthService, IProfileService, IResourceOwnerPasswordValidator
    {
        private readonly IHasher hasher;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IUsersSearchService usersSearchService;
        private readonly IUsersRepository usersRepository;
        private readonly IDistributedCache cache;

        public AuthService(IHasher hasher,
            ISecurityRolesRepository rolesRepository,
            IUsersSearchService usersSearchService,
            IUsersRepository usersRepository, IDistributedCache cache)
        {
            this.hasher = hasher;
            this.rolesRepository = rolesRepository;
            this.usersSearchService = usersSearchService;
            this.usersRepository = usersRepository;
            this.cache = cache;
        }
        
        public async Task RegisterAsync(User user)
        {            
            await this.usersRepository.AddAsync(user);

            await this.cache.AddOrUpdateAsync(user.Id, user);

            foreach (var email in user.Emails)
            {
                await this.cache.AddOrUpdateAsync(email.Id, email);
            }
        }
        
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims = context.Subject.Claims.ToList();

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(context.IsActive);
        }
        
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var loginUser = await this.usersSearchService.FindUserByUserNameOrEmailAsync(context.UserName);

            if (loginUser == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            if (!this.ArePasswordsEqual(loginUser.Password, context.Password, loginUser.Salt))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            context.Result = new GrantValidationResult(loginUser.Id.ToString(), 
                "custom", 
                await this.AllocateClaims(loginUser));
        }
        
        private async Task<IEnumerable<Claim>> AllocateClaims(User user)
        {
            if (user == null)
            {
                throw new ArgumentException(nameof(user));
            }
            
            var claims = new List<Claim>();

            foreach (var roleId in user.RoleIds)
            {
                var existingRole = await this.rolesRepository.FindAsync(roleId)
                    .ConfigureAwait(false);
                    
                claims.Add(new Claim(JwtClaimTypes.Role, existingRole.Name));
            }

            claims.AddRange(new[]
            {
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
            });

            claims.AddRange(user.Emails.Select(email => new Claim(JwtClaimTypes.Email, email.Address)).ToArray());

            if (user.FirstName != null && user.Name != null)
            {
                claims.AddRange(new []
                {
                    new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.Name}"),
                    new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, user.Name)
                });
            }

            return claims;
        }
        
        private bool ArePasswordsEqual(string hashedPassword, string unhasedPassword, byte[] salt)
        {
            return this.hasher.CreateSaltedHash(unhasedPassword, salt) == hashedPassword;
        }
    }
}