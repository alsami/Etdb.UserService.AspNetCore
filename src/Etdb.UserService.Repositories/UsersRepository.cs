using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.DocumentRepository.Generics;
using Etdb.UserService.Domain;
using Etdb.UserService.Repositories.Abstractions;
using IdentityModel;
using MongoDB.Driver;

// ReSharper disable SpecifyStringComparison

namespace Etdb.UserService.Repositories
{
    public class UsersRepository : GenericDocumentRepository<User, Guid>, IUsersRepository
    {
        private readonly DocumentDbContext context;

        public UsersRepository(DocumentDbContext context) : base(context)
        {
            this.context = context;
        }
        
        public async Task<IEnumerable<Claim>> AllocateClaims(User user)
        {
            if (user == null) throw new ArgumentException(nameof(user));
            
            var claims = new List<Claim>();

            foreach (var roleRef in user.SecurityRoleReferences)
            {
                var filter = Builders<SecurityRole>.Filter.Eq(role => role.Id, roleRef.Id.AsGuid);
                
                var existingRole = await this.context.Database.GetCollection<SecurityRole>($"{nameof(SecurityRole).ToLower()}s")
                    .Find(filter)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
                    
                claims.Add(new Claim(JwtClaimTypes.Role, existingRole.Name));
            }

            claims.AddRange(new[]
            {
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
            });

            claims.AddRange(user.Emails.Select(email => new Claim(JwtClaimTypes.Email, email.Address)).ToArray());

            if (user.FirstName != null && user.LastName != null)
            {
                claims.AddRange(new []
                {
                    new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, user.LastName),
                });
            }

            return claims;
        }

        public async Task<User> FindUserAsync(Guid id)
        {
            return await this.FindAsync(id);
        }

        public async Task<User> FindUserAsync(string userName)
        {
            return await this.FindAsync(user => user.UserName.ToLower() == userName.ToLower())
                .ConfigureAwait(false);
        }

        public async Task<User> FindUserAsync(string userName, string emailAddress)
        {
            return await this.FindAsync(user => user.UserName.ToLower() == userName.ToLower() ||
                                                user.Emails.Any(email => email.Address.ToLower() == emailAddress.ToLower()))
                .ConfigureAwait(false);
        }

        public async Task RegisterAsync(User user)
        {
            await this.AddAsync(user);
        }
    }
}