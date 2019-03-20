using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Extensions;
using Etdb.UserService.Options;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Services
{
    // ReSharper disable SpecifyStringComparison
    public class UsersService : IUsersService
    {
        private readonly IDistributedCache cache;
        private readonly IUsersRepository usersRepository;
        private readonly IFileService fileService;
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;

        public UsersService(IUsersRepository usersRepository, IDistributedCache cache, IFileService fileService,
            IOptions<FilestoreConfiguration> fileStoreOptions)
        {
            this.usersRepository = usersRepository;
            this.cache = cache;
            this.fileService = fileService;
            this.fileStoreOptions = fileStoreOptions;
        }

        public async Task AddAsync(User user)
        {
            await this.usersRepository.AddAsync(user);

            await this.cache.AddOrUpdateAsync(user.Id, user);

            var emailCachingTasks = user
                .Emails
                .Select(async email => await this.cache.AddOrUpdateAsync(email.Id, email))
                .ToArray();

            await Task.WhenAll(emailCachingTasks);
        }

        public async Task EditAsync(User user)
        {
            var saved = await this.usersRepository.EditAsync(user);

            if (!saved)
            {
                return;
            }

            await this.cache.AddOrUpdateAsync(user.Id, user);
        }

        public async Task<User> EditProfileImageAsync(User user, UserProfileImage userProfileImage, byte[] file)
        {
            if (user.ProfileImage != null)
            {
                this.fileService.DeleteBinary(Path.Combine(this.fileStoreOptions.Value.ImagePath,
                    user.Id.ToString(),
                    user.ProfileImage.Name));
            }

            await this.fileService.StoreBinaryAsync(
                Path.Combine(this.fileStoreOptions.Value.ImagePath, user.Id.ToString()), userProfileImage.Name,
                file);

            var updatedUser = new User(user.Id, user.UserName, user.FirstName,
                user.Name, user.Biography,
                user.RegisteredSince, user.RoleIds, user.Emails,
                user.AuthenticationProvider,
                user.Password, user.Salt, userProfileImage);

            await this.EditAsync(updatedUser);

            return updatedUser;
        }

        public async Task<User> FindByIdAsync(Guid id)
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

        public async Task<User> FindByUserNameAsync(string userName)
        {
            var user = await this.usersRepository.FindAsync(UserNameEqualsExpression(userName));

            if (user != null)
            {
                await this.cache.AddOrUpdateAsync(user.Id, user);
            }

            return user;
        }

        public async Task<User> FindByUserNameOrEmailAsync(string userNameOrEmail)
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
                .FirstOrDefault(EmailEqualsExpression(emailAddress));

            return email;
        }

        private static Expression<Func<User, bool>> UserNameEqualsExpression(string userName) =>
            user => user.UserName.ToLower() == userName.ToLower();

        private static Expression<Func<Email, bool>> EmailEqualsExpression(string emailAddress) =>
            email => email.Address.ToLower() == emailAddress.ToLower();

        private static Expression<Func<User, bool>> UserOrEmailEqualsExpression(string userNameOrEmail) => user =>
            user.UserName.ToLower() == userNameOrEmail.ToLower() ||
            user.Emails.Any(email => email.Address.ToLower() == userNameOrEmail.ToLower());
    }
}