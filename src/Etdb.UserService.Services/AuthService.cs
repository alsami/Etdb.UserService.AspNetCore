using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Constants;
using Etdb.UserService.Domain;
using Etdb.UserService.Extensions;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

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
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException(nameof(user.Password));
            }

            user.Id = user.Id == Guid.Empty ? Guid.NewGuid() : user.Id;
            
            foreach (var email in user.Emails)
            {
                email.Id = email.Id == Guid.Empty ? Guid.NewGuid() : email.Id;;
                email.UserId = user.Id;
            }
            
            var memberRole = await this.rolesRepository.FindAsync(role => role.Name == RoleNames.Member);

            user.SecurityRoleReferences.Add(new MongoDBRef($"{ nameof(SecurityRole).ToLower() }s", memberRole.Id));

            var salt = this.hasher.GenerateSalt();

            user.Password = this.hasher.CreateSaltedHash(user.Password, salt);

            user.Salt = salt;
            
            await this.usersRepository.AddAsync(user);

            await this.cache.AddOrUpdateAsync(user.Id, user);
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

            if (!this.ArePasswordsEqual(loginUser, context.Password))
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

            foreach (var roleRef in user.SecurityRoleReferences)
            {
                var existingRole = await this.rolesRepository.FindAsync(role => role.Id.Equals(roleRef.Id.AsGuid))
                    .ConfigureAwait(false);
                    
                claims.Add(new Claim(JwtClaimTypes.Role, existingRole.Name));
            }

            claims.AddRange(new[]
            {
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
            });

            claims.AddRange(user.Emails.Select(email => new Claim(JwtClaimTypes.Email, email.Address)).ToArray());

            if (user.FirstName != null && user.LastName != null)
            {
                claims.AddRange(new []
                {
                    new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, user.LastName),
                });
            }

            return claims;
        }
        
        private bool ArePasswordsEqual(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(); 
            }

            return this.hasher.CreateSaltedHash(password, user.Salt) == user.Password;
        }
    }
}