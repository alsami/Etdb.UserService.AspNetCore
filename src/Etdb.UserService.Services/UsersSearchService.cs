using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Domain;
using Etdb.UserService.Extensions;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using IdentityModel;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace Etdb.UserService.Services
{
    // ReSharper disable SpecifyStringComparison
    public class UsersSearchService : IUsersSearchService
    {
        private readonly IDistributedCache cache;
        private readonly IUsersRepository usersRepository;
        

        public UsersSearchService(IUsersRepository usersRepository, IDistributedCache cache)
        {
            this.usersRepository = usersRepository;
            this.cache = cache;
        }
        
        public async Task<User> FindUserByIdAsync(Guid id)
        {
            var cachedUser = await this.cache.GetAsync<User, Guid>(id);

            if (cachedUser != null)
            {
                await this.cache.RefreshAsync(id.ToString());
                
                return cachedUser;
            }
            
            var user = await this.usersRepository.FindAsync(id);

            if (user != null)
            {
                await this.cache.AddOrUpdateAsync(user.Id, user);
            }

            return user;
        }

        public async Task<User> FindUserByUserNameAsync(string userName)
        {            
            var user = await this.usersRepository.FindAsync(UserNameEqualsExpression(userName));

            if (user != null)
            {
                await this.cache.AddOrUpdateAsync(user.Id, user);
            }

            return user;
        }

        public async Task<User> FindUserByUserNameOrEmailAsync(string userNameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(userNameOrEmail))
            {
                throw new ArgumentException(nameof(userNameOrEmail));
            }
            
            var user = await this.usersRepository.FindAsync(UserOrEmailEqualsExpression(userNameOrEmail));

            if (user != null)
            {
                await this.cache.AddOrUpdateAsync(user.Id, user);
            }

            return user;
        }

        public async Task<Email> FindEmailAddress(string emailAddress)
        {
            var entry = await this.cache.GetAsync<Email, string>(emailAddress.ToLower());

            if (entry != null)
            {
                await this.cache.RefreshAsync(emailAddress);
                return entry;
            }
            
            var email = this.usersRepository.Query()
                .SelectMany(user => user.Emails)
                .FirstOrDefault(EmailEqualsExpressios(emailAddress));

            if (email != null)
            {
                await this.cache.AddOrUpdateAsync(email.Address, email, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
            }

            return email;
        }

        private static Expression<Func<User, bool>> UserNameEqualsExpression(string userName) => 
            user => user.UserName.ToLower() == userName.ToLower();

        private static Expression<Func<User, bool>> UserHasAnyEqualEmailExpression(string emailAddress) =>
            user => user.Emails.Any(email => email.Address.ToLower() == emailAddress.ToLower());

        private static Expression<Func<Email, bool>> EmailEqualsExpressios(string emailAddress) =>
            email => email.Address.ToLower() == emailAddress.ToLower();

        private static Expression<Func<User, bool>> UserOrEmailEqualsExpression(string userNameOrEmail) => user =>
            user.UserName.ToLower() == userNameOrEmail.ToLower() ||
            user.Emails.Any(email => email.Address.ToLower() == userNameOrEmail.ToLower());
    }
}