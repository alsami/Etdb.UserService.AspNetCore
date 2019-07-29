using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Etdb.UserService.Presentation.Authentication;
using IdentityModel;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class IdentityUserTypeConverter : ITypeConverter<IEnumerable<Claim>, IdentityUserDto>
    {
        public IdentityUserDto Convert(IEnumerable<Claim> source, IdentityUserDto destination,
            ResolutionContext context)
        {
            var claims = source as Claim[] ?? source.ToArray();
            var firstName = claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.GivenName)?.Value;
            var lastName = claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.FamilyName)?.Value;

            return new IdentityUserDto(Guid.Parse(claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value),
                firstName, lastName,
                claims.First(claim => claim.Type == JwtClaimTypes.PreferredUserName).Value,
                claims
                    .First(claim => claim.Type == JwtClaimTypes.Email).Value,
                claims
                    .Where(claim => claim.Type == JwtClaimTypes.Role)
                    .Select(claim => claim.Value)
                    .ToArray(), claims.First(claim => claim.Type == JwtClaimTypes.IdentityProvider).Value,
                claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Picture)?.Value);
        }
    }
}