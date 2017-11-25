using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ETDB.API.ServiceBase.Abstractions.Repositories;
using ETDB.API.ServiceBase.Constants;
using ETDB.API.UserService.Domain;
using ETDB.API.UserService.Domain.Entities;
using IdentityModel;

namespace ETDB.API.UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IEntityRepository<User> userBaseRepo;
        private readonly IEntityRepository<Securityrole> securityRoleRepo;
        private readonly IEntityRepository<UserSecurityrole> userSecurityroleBaseRepo;

        public UserRepository(IEntityRepository<UserSecurityrole> userSecurityroleBaseRepo, 
            IEntityRepository<User> userBaseRepo, IEntityRepository<Securityrole> securityRoleRepo)
        {
            this.userSecurityroleBaseRepo = userSecurityroleBaseRepo;
            this.userBaseRepo = userBaseRepo;
            this.securityRoleRepo = securityRoleRepo;
        }

        public IEnumerable<Claim> GetClaims(User user)
        {
            if(user == null) throw new ArgumentException(nameof(user));

            if (!user.UserSecurityroles.Any() 
                || user.UserSecurityroles.Any(userSecurityrole => userSecurityrole?.Securityrole == null))
            {
                user.UserSecurityroles = this.userSecurityroleBaseRepo
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

        public User Get(string userName)
        {
            return this.userBaseRepo.Get(user => 
                user.Email.Equals(userName, StringComparison.OrdinalIgnoreCase) 
                || user.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        public User Get(string userName, string email)
        {
            return this.userBaseRepo.Get(user =>
                user.Email.Equals(userName, StringComparison.OrdinalIgnoreCase)
                || user.UserName.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void Register(User user)
        {
            user.UserSecurityroles.Add(new UserSecurityrole
            {
                User = user,
                Securityrole = this.securityRoleRepo.Get(role => role.Designation == RoleNames.User)
            });

            this.userBaseRepo.Add(user);
        }
    }
}
