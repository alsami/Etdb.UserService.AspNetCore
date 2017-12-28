using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Constants;
using Etdb.ServiceBase.Repositories.Abstractions.Base;
using Etdb.ServiceBase.Repositories.Generics;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.EntityInfos;
using Etdb.UserService.Repositories.Abstractions;
using IdentityModel;

namespace Etdb.UserService.Repositories
{
    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        private readonly ISecurityRoleRepository securityRoleRepo;

        public UserRepository(AppContextBase context, ISecurityRoleRepository securityRoleRepo) : base(context)
        {
            this.securityRoleRepo = securityRoleRepo;
        }

        public IEnumerable<Claim> GetClaims(User user)
        {
            if (user == null) throw new ArgumentException(nameof(user));


            var claims = user
                .SecurityRoles
                .Select(role => new Claim(JwtClaimTypes.Role, role.Designation))
                .ToList();

            claims.AddRange(new[]
            {
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtClaimTypes.Email, user.Email),
            });

            if (user.Name != null && user.LastName != null)
            {
                claims.AddRange(new []
                {
                    new Claim(JwtClaimTypes.Name, $"{user.Name} {user.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, user.Name),
                    new Claim(JwtClaimTypes.FamilyName, user.LastName),
                });
            }

            return claims;
        }

        public async Task<User> FindAsync(string userName)
        {
            var existingUser = await this.GetAsync(user => user.UserName == userName);

            return existingUser;
        }

        public async Task<User> FindAsync(string userName, string email)
        {
            var existingUser = await this.GetAsync(user => user.Email == email || user.UserName == userName);

            return existingUser;
        }

        public async Task RegisterAsync(User user)
        {
            var memberRole = await this.securityRoleRepo.FindAsync(RoleNames.Member);

            if (memberRole == null)
            {
                throw new Exception($"You must setup a role with designation {RoleNames.Member}");
            }

            user.SecurityRoles.Add(new SecurityRoleInfo
            {
                Designation = memberRole.Designation,
                Id = memberRole.Id
            });

            var currenDate = DateTime.UtcNow;

            user.CreatedAt = currenDate;
            user.UpdatedAt = currenDate;

            await this.AddAsync(user);
        }
    }
}
