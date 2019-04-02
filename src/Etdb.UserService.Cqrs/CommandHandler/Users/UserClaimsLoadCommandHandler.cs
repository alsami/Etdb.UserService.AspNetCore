using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class UserClaimsLoadCommandHandler : IResponseCommandHandler<UserClaimsLoadCommand, IEnumerable<Claim>>
    {
        private readonly IUsersService usersService;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IProfileImageUrlFactory profileImageUrlFactory;

        public UserClaimsLoadCommandHandler(ISecurityRolesRepository rolesRepository,
            IProfileImageUrlFactory profileImageUrlFactory, IUsersService usersService)
        {
            this.rolesRepository = rolesRepository;
            this.profileImageUrlFactory = profileImageUrlFactory;
            this.usersService = usersService;
        }

        public async Task<IEnumerable<Claim>> Handle(UserClaimsLoadCommand command, CancellationToken cancellationToken)
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

            if (user.FirstName != null && user.Name != null)
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.Name}"),
                    new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, user.Name)
                });
            }

            if (user.ProfileImages.Any())
            {
                claims.Add(new Claim(JwtClaimTypes.Picture,
                    this.profileImageUrlFactory.GenerateUrl(user)));
            }

            claims.Add(new Claim(JwtClaimTypes.IdentityProvider, user.AuthenticationProvider.ToString()));

            return claims;
        }
    }
}