using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;
using MediatR;

namespace Etdb.UserService.Cqrs.CommandHandler.Authentication
{
    public class ClaimsLoadCommandHandler : IRequestHandler<ClaimsLoadCommand, IEnumerable<Claim>>
    {
        private readonly IUsersService usersService;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IProfileImageUrlFactory profileImageUrlFactory;

        public ClaimsLoadCommandHandler(ISecurityRolesRepository rolesRepository, IUsersService usersService,
            IProfileImageUrlFactory profileImageUrlFactory)
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

                claims.Add(new Claim(JwtClaimTypes.Role, existingRole!.Name));
            }

            claims.AddRange(new[]
            {
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
            });

            claims.Add(new Claim(JwtClaimTypes.Email, user.Emails.First(email => email.IsPrimary).Address));

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
                    this.profileImageUrlFactory.GetResizeUrl(usedImage, user.Id)));
            }

            claims.Add(new Claim(JwtClaimTypes.IdentityProvider, user.AuthenticationProvider.ToString()));

            return claims;
        }
    }
}