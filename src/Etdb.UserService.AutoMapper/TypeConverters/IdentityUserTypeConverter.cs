using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Etdb.UserService.Presentation;
using IdentityModel;

namespace Etdb.UserService.AutoMapper.TypeConverters
{
    public class IdentityUserTypeConverter : ITypeConverter<IEnumerable<Claim>, IdentityUserDto>
    {
        public IdentityUserDto Convert(IEnumerable<Claim> source, IdentityUserDto destination,
            ResolutionContext context)
        {
            var claims = source as Claim[] ?? source.ToArray();
            var firstName = claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.GivenName)?.Value;
            var lastName = claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.FamilyName)?.Value;

            return new IdentityUserDto
            {
                Id = Guid.Parse(claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value),
                FirstName = firstName,
                LastName = lastName,
                FullName = $"{firstName} {lastName}",
                UserName = claims.First(claim => claim.Type == JwtClaimTypes.PreferredUserName).Value,
                Emails = claims
                    .Where(claim => claim.Type == JwtClaimTypes.Email)
                    .Select(claim => claim.Value)
                    .ToArray(),
                Roles = claims
                    .Where(claim => claim.Type == JwtClaimTypes.Role)
                    .Select(claim => claim.Value)
                    .ToArray(),
                AuthenticationProvider = claims.First(claim => claim.Type == JwtClaimTypes.IdentityProvider).Value,
                ProfileImageUrl = claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Picture)?.Value,
            };
        }
    }
}