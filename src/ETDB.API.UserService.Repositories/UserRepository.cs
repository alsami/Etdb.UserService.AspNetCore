using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ETDB.API.ServiceBase.Constants;
using ETDB.API.ServiceBase.Repositories.Abstractions.Base;
using ETDB.API.ServiceBase.Repositories.Generics;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Repositories.Abstractions;
using IdentityModel;

namespace ETDB.API.UserService.Repositories
{
    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        private readonly IUserSecurityRoleRepository userSecurityroleRepo;
        private readonly ISecurityRoleRepository securityRoleRepo;

        public UserRepository(AppContextBase context, IUserSecurityRoleRepository userSecurityroleRepo, 
            ISecurityRoleRepository securityRoleRepo) : base(context)
        {
            this.userSecurityroleRepo = userSecurityroleRepo;
            this.securityRoleRepo = securityRoleRepo;
        }

        public IEnumerable<Claim> GetClaims(User user)
        {
            if (user == null) throw new ArgumentException(nameof(user));

            if (!user.UserSecurityroles.Any()
                || user.UserSecurityroles.Any(userSecurityrole => userSecurityrole?.Securityrole == null))
            {
                user.UserSecurityroles = this.userSecurityroleRepo
                    .GetAllIncluding(userSecurityrole => userSecurityrole.UserId == user.Id,
                        userSecurityrole => userSecurityrole.Securityrole)
                    .ToList();
            }

            var claims = user
                .UserSecurityroles
                .Select(role => new Claim(JwtClaimTypes.Role, role.Securityrole.Designation))
                .ToList();

            claims.AddRange(new[]
            {
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtClaimTypes.Name, $"{user.Name} {user.LastName}"),
                new Claim(JwtClaimTypes.GivenName, user.Name),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Email, user.Email),
            });

            return claims;
        }

        public User Find(string userName)
        {
            var existingUser = this.Get(user => user.UserName == userName);

            return existingUser;
        }

        public User Find(string userName, string email)
        {
            var existingUser = this.Get(user => user.Email == email || user.UserName == userName);

            return existingUser;
        }

        public void Register(User user)
        {
            user.UserSecurityroles.Add(new UserSecurityrole
            {
                User = user,
                Securityrole = this.securityRoleRepo.Find(RoleNames.Member)
            });

            this.Add(user);
        }
    }
}
