using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Etdb.UserService.Services
{
    public class AuthService : IProfileService, IResourceOwnerPasswordValidator
    {
        private readonly IHasher hasher;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IUsersService userService;
        private readonly IUserProfileImageUrlFactory userProfileImageUrlFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILoginLogRepository loginLogRepository;
        private readonly ILogger<AuthService> logger;

        public AuthService(IHasher hasher,
            ISecurityRolesRepository rolesRepository,
            IUsersService userService, IUserProfileImageUrlFactory userProfileImageUrlFactory,
            IHttpContextAccessor httpContextAccessor, ILoginLogRepository loginLogRepository,
            ILogger<AuthService> logger)
        {
            this.hasher = hasher;
            this.rolesRepository = rolesRepository;
            this.userService = userService;
            this.userProfileImageUrlFactory = userProfileImageUrlFactory;
            this.httpContextAccessor = httpContextAccessor;
            this.loginLogRepository = loginLogRepository;
            this.logger = logger;
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

        public async Task IsActiveAsync(IsActiveContext context)
        {
            await Task.FromResult(context.IsActive);
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var loginUser = await this.userService.FindByUserNameOrEmailAsync(context.UserName);

            if (loginUser == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            var passwordIsValid = this.hasher.CreateSaltedHash(context.Password, loginUser.Salt)
                                  == loginUser.Password;

            if (!passwordIsValid)
            {
                await this.LogLoginEvent(LoginType.Failed, loginUser.Id, "Given password is invalid!");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            await this.LogLoginEvent(LoginType.Succeeded, loginUser.Id);

            context.Result = new GrantValidationResult(loginUser.Id.ToString(),
                "custom",
                await this.AllocateClaimsAsync(loginUser));
        }

        private async Task LogLoginEvent(LoginType loginType, Guid userId, string additionalInfo = null)
        {
            var log = new LoginLog(Guid.NewGuid(), userId, DateTime.UtcNow, loginType,
                this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(), additionalInfo);

            try
            {
                await this.loginLogRepository.AddAsync(log);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, ex.Message);
            }
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
    }
}