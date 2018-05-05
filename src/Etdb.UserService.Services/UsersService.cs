using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Domain;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;
using MongoDB.Driver;

namespace Etdb.UserService.Services
{
    // ReSharper disable SpecifyStringComparison
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IHasher hasher;

        public UsersService(IUsersRepository usersRepository, ISecurityRolesRepository rolesRepository, IHasher hasher)
        {
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
            this.hasher = hasher;
        }
        
        public async Task<IEnumerable<Claim>> AllocateClaims(User user)
        {
            if (user == null)
            {
                throw new ArgumentException(nameof(user));
            }
            
            var claims = new List<Claim>();

            foreach (var roleRef in user.SecurityRoleReferences)
            {
                var existingRole = await this.rolesRepository.FindAsync(role => role.Id.Equals(roleRef.Id.AsGuid))
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

        public async Task<User> FindUserByIdAsync(Guid id)
        {
            return await this.usersRepository.FindAsync(id);
        }

        public async Task<User> FindUserByUserNameAsync(string userName)
        {
            return await this.usersRepository.FindAsync(UserNameEqualsExpression(userName));
        }

        public async Task<User> FindUserByUserNameOrEmailAsync(string userNameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(userNameOrEmail))
            {
                throw new ArgumentException(nameof(userNameOrEmail));
            }
            
            return await this.usersRepository.FindAsync(user => user.UserName.ToLower() == userNameOrEmail.ToLower()
                                                               || user.Emails.Any(email => email.Address.ToLower() == userNameOrEmail.ToLower()));
        }

        public async Task RegisterAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException(nameof(user.Password));
            }
            
            var memberRole = await this.rolesRepository.FindAsync(role => role.Name == RoleNames.Member);

            user.SecurityRoleReferences.Add(new MongoDBRef($"{ nameof(SecurityRole).ToLower() }s", memberRole.Id));

            var salt = this.hasher.GenerateSalt();

            user.Password = this.hasher.CreateSaltedHash(user.Password, salt);

            user.Salt = salt;
            
            await this.usersRepository.AddAsync(user);
        }

        public Email FindEmailAddress(string emailAddress)
        {
            return this.usersRepository.Query()
                .SelectMany(user => user.Emails)
                .FirstOrDefault(EmailEqualsExpressios(emailAddress));
        }

        public bool ArePasswordsEqual(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(); 
            }

            return this.hasher.CreateSaltedHash(password, user.Salt) == user.Password;
        }

        private static Expression<Func<User, bool>> UserNameEqualsExpression(string userName) => 
            user => user.UserName.ToLower() == userName.ToLower();

        private static Expression<Func<User, bool>> UserHasAnyEqualEmailExpression(string emailAddress) =>
            user => user.Emails.Any(email => email.Address.ToLower() == emailAddress.ToLower());

        private static Expression<Func<Email, bool>> EmailEqualsExpressios(string emailAddress) =>
            email => email.Address.ToLower() == emailAddress.ToLower();
    }
}