using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;

namespace Etdb.UserService.Cqrs.CommandHandler.Authentication
{
    public class ClaimsLoadCommandHandler : IResponseCommandHandler<ClaimsLoadCommand, IEnumerable<Claim>>
    {
        private readonly IUsersService usersService;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IUserUrlFactory profileImageUrlFactory;

        public ClaimsLoadCommandHandler(ISecurityRolesRepository rolesRepository, IUsersService usersService, IUserUrlFactory profileImageUrlFactory)
        {
            this.rolesRepository = rolesRepository;
            this.usersService = usersService;
            this.profileImageUrlFactory = profileImageUrlFactory;
        }

        public async Task<IEnumerable<Claim>> Handle(ClaimsLoadCommand command, CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(command.Id);

            if (user == null)
            {
                return Array.Empty<Claim>();
            }

            return await this.AllocateClaimsAsync(user);
        }

        private async Task<IEnumerable<Claim>> AllocateClaimsAsync(User user)
        {
            var claims = new List<Claim>();

            foreach (var roleId in user.RoleIds)
            {
                var existingRole = await this.rolesRepository.FindAsync(roleId)
                    .ConfigureAwait(false);

                claims.Add(new Claim(JwtClaimTypes.Role, existingRole.Name));
            }

            claims.AddRange(new[]
            {
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
            });

            claims.AddRange(user.Emails.Select(email => new Claim(JwtClaimTypes.Email, email.Address)).ToArray());

            if (user.FirstName != null)
            {
                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            }

            if (user.Name != null)
            {
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.Name));
            }

            if (user.FirstName != null && user.Name != null)
            {
                claims.Add(new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.Name}"));
            }

            if (user.ProfileImages.Any())
            {
                var usedImage = user.ProfileImages.FirstOrDefault(image => image.IsPrimary) ??
                                user.ProfileImages.First();
                
                claims.Add(new Claim(JwtClaimTypes.Picture,
                    this.profileImageUrlFactory.GenerateUrlWithChildIdParameter(
                        usedImage,
                        RouteNames.ProfileImages.LoadRoute)));
            }

            claims.Add(new Claim(JwtClaimTypes.IdentityProvider, user.AuthenticationProvider.ToString()));

            return claims;
        }
    }
}