using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MediatR;

namespace Etdb.UserService.Authentication.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IMediator bus;

        public ProfileService(IMediator bus)
        {
            this.bus = bus;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Subject)?.Value;

            if (!Guid.TryParse(subject, out var userId))
            {
                context.IssuedClaims = context.Subject.Claims.ToList();
                return;
            }

            var claims =
                await this.bus.Send(
                    new ClaimsLoadCommand(userId));

            var enumeratedClaims = claims as Claim[] ?? claims.ToArray();

            context.IssuedClaims = enumeratedClaims.Any()
                ? enumeratedClaims.ToList()
                : context.IssuedClaims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            await Task.FromResult(context.IsActive);
        }
    }
}