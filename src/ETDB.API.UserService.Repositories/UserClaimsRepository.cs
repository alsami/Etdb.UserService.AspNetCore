using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ETDB.API.ServiceBase.Abstractions.Repositories;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Repositories.Base;
using IdentityModel;

namespace ETDB.API.UserService.Repositories
{
    public class UserClaimsRepository : IUserClaimsRepository
    {
        private readonly IEntityRepository<UserSecurityrole> userSecurityroleRepository;

        public UserClaimsRepository(IEntityRepository<UserSecurityrole> userSecurityroleRepository)
        {
            this.userSecurityroleRepository = userSecurityroleRepository;
        }

        public IEnumerable<Claim> GetClaims(User user)
        {
            if(user == null) throw new ArgumentException(nameof(user));

            if (!user.UserSecurityroles.Any() 
                || user.UserSecurityroles.Any(userSecurityrole => userSecurityrole?.Securityrole == null))
            {
                user.UserSecurityroles = this.userSecurityroleRepository
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
    }
}
