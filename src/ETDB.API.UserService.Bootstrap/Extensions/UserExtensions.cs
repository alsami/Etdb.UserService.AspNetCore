using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ETDB.API.UserService.Domain.Entities;
using IdentityModel;

namespace ETDB.API.UserService.Bootstrap.Extensions
{
    public static class UserExtensions
    {
        public static ICollection<Claim> GetClaims(this User user)
        {
            var claims = user
                .UserSecurityroles
                .Select(role => new Claim(JwtClaimTypes.Role, role.Securityrole.Designation))
                .ToList();

            claims.AddRange(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtClaimTypes.Name, $"{user.Name} {user.LastName}"),
                new Claim(JwtClaimTypes.GivenName, user.Name),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Email, user.Email),
            });

            return claims;
        }
    }
}
