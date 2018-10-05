using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Extensions;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Etdb.UserService.Services
{
    // ReSharper disable SpecifyStringComparison
    public class UsersService : IUsersService
    {
        private readonly IDistributedCache cache;
        private readonly IUsersRepository usersRepository;


        public UsersService(IUsersRepository usersRepository, IDistributedCache cache)
        {
            this.usersRepository = usersRepository;
            this.cache = cache;
        }

        public async Task AddAsync(User user)
        {
            await this.usersRepository.AddAsync(user);

            await this.cache.AddOrUpdateAsync(user.Id, user);

            foreach (var email in user.Emails)
            {
                await this.cache.AddOrUpdateAsync(email.Id, email);
            }
        }

        public async Task<bool> EditUserAsync(User user)
        {
            var saved = await this.usersRepository.EditAsync(user);

            if (!saved)
            {
                return false;
            }

            await this.cache.AddOrUpdateAsync(user.Id, user);

            return true;
        }

        public async Task<User> FindUserByIdAsync(Guid id)
        {
            var cachedUser = await this.cache.FindAsync<User, Guid>(id);

            if (cachedUser != null)
            {
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

        public Email FindEmailAddress(string emailAddress)
        {
            var email = this.usersRepository
                .Query()
                .SelectMany(user => user.Emails)
                .FirstOrDefault(EmailEqualsExpressios(emailAddress));

            return email;
        }

        private static Expression<Func<User, bool>> UserNameEqualsExpression(string userName) =>
            user => user.UserName.ToLower() == userName.ToLower();

        private static Expression<Func<Email, bool>> EmailEqualsExpressios(string emailAddress) =>
            email => email.Address.ToLower() == emailAddress.ToLower();

        private static Expression<Func<User, bool>> UserOrEmailEqualsExpression(string userNameOrEmail) => user =>
            user.UserName.ToLower() == userNameOrEmail.ToLower() ||
            user.Emails.Any(email => email.Address.ToLower() == userNameOrEmail.ToLower());
    }
}