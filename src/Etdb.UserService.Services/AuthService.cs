using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;

namespace Etdb.UserService.Services
{
    public class AuthService : IProfileService, IResourceOwnerPasswordValidator
    {
        private readonly IHasher hasher;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IUsersService userService;
        private readonly IUserProfileImageUrlFactory userProfileImageUrlFactory;

        public AuthService(IHasher hasher,
            ISecurityRolesRepository rolesRepository,
            IUsersService userService, IUserProfileImageUrlFactory userProfileImageUrlFactory)
        {
            this.hasher = hasher;
            this.rolesRepository = rolesRepository;
            this.userService = userService;
            this.userProfileImageUrlFactory = userProfileImageUrlFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (!Guid.TryParse(
                context.Subject.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Subject)?.Value,
                out var subjectId))
            {
                context.IssuedClaims = context.Subject.Claims.ToList();
                return;
            }

            var user = await this.userService.FindByIdAsync(subjectId);

            if (user == null)
            {
                context.IssuedClaims = context.Subject.Claims.ToList();
                return;
            }

            context.IssuedClaims = (await this.AllocateClaimsAsync(user)).ToList();
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(context.IsActive);
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var loginUser = await this.userService.FindByUserNameOrEmailAsync(context.UserName);

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
                await this.AllocateClaimsAsync(loginUser));
        }

        private async Task<IEnumerable<Claim>> AllocateClaimsAsync(User user)
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
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.Name}"),
                    new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, user.Name)
                });
            }

            if (user.ProfileImage != null)
            {
                claims.Add(new Claim(JwtClaimTypes.Picture,
                    this.userProfileImageUrlFactory.GenerateUrl(user.Id, user.ProfileImage)));
            }

            return claims;
        }

        private bool ArePasswordsEqual(string hashedPassword, string unhasedPassword, byte[] salt)
        {
            return this.hasher.CreateSaltedHash(unhasedPassword, salt) == hashedPassword;
        }
    }
}