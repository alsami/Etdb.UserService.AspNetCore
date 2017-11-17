using System;
using System.Linq;
using System.Threading.Tasks;
using ETDB.API.ServiceBase.Generics.Base;
using ETDB.API.UserService.Bootstrap.Extensions;
using ETDB.API.UserService.Domain.Entities;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace ETDB.API.UserService.Bootstrap.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IEntityRepository<User> userRepository;

        public ProfileService(IEntityRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Subject);

            if (!Guid.TryParse(subject?.Value, out var userId))
            {
                return Task.FromResult(0);
            }

            var loginUser = this.userRepository
                .Get(userId);

            if (loginUser == null)
            {
                return Task.FromResult(0);
            }

            context.IssuedClaims = loginUser
                .GetClaims()
                .ToList();

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Subject);

            if (!Guid.TryParse(subject?.Value, out var userId))
            {
                return Task.FromResult(0);
            }

            var loginUser = this.userRepository
                .Get(userId);

            context.IsActive = loginUser?.IsActive ?? false;
            return Task.FromResult(0);
        }
    }
}
