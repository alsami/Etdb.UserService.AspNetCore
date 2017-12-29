using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Repositories.Abstractions.Base;
using Etdb.ServiceBase.Repositories.Generics;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;
using IdentityModel;

namespace Etdb.UserService.Repositories
{
    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        private readonly ISecurityRoleRepository securityRoleRepository;

        public UserRepository(AppContextBase context, ISecurityRoleRepository securityRoleRepository) : base(context)
        {
            this.securityRoleRepository = securityRoleRepository;
        }

        public async Task<IEnumerable<Claim>> GetClaims(User user)
        {
            if (user == null) throw new ArgumentException(nameof(user));
            
            var claims = new List<Claim>();

            foreach (var securityRole in user.SecurityRoles)
            {
                var existingRole = await this.securityRoleRepository.GetAsync(Guid.Parse(securityRole.Id.ToString()));
                claims.Add(new Claim(JwtClaimTypes.Role, existingRole.Designation));
            }

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
            var currenDate = DateTime.UtcNow;

            user.CreatedAt = currenDate;
            user.UpdatedAt = currenDate;

            await this.AddAsync(user);
        }
    }
}
