using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using Etdb.UserService.Services.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Services
{
    // ReSharper disable SpecifyStringComparison
    public class UsersService : IUsersService
    {

        private readonly IUsersRepository usersRepository;
        private readonly IProfileImageStorageService profileImageStorageService;

        public UsersService(IUsersRepository usersRepository, IProfileImageStorageService profileImageStorageService)
        {
            this.usersRepository = usersRepository;
            this.profileImageStorageService = profileImageStorageService;
        }

        public async Task AddAsync(User user, params StorableImage[] storeImageMetaInfos)
        {
            if (storeImageMetaInfos.Any()) await this.StoreProfileImagesAsync(user, storeImageMetaInfos);

            await this.usersRepository.AddAsync(user);
        }

        public async Task EditAsync(User user, params StorableImage[] storeImageMetaInfos)
        {
            if (storeImageMetaInfos.Any())
                await this.StoreProfileImagesAsync(user, storeImageMetaInfos);

            await this.usersRepository.EditAsync(user);
        }

        public Task EditAsync(User user)
        {
            return this.usersRepository.EditAsync(user);
        }

        public Task<User?> FindByIdAsync(Guid id)
        {
            return this.usersRepository.FindAsync(id);
        }

        public Task<User?> FindByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException(nameof(userName));

            return this.usersRepository.FindAsync(UserNameEqualsExpression(userName));
        }

        public Task<User?> FindByUserNameOrEmailAsync(string userNameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(userNameOrEmail)) throw new ArgumentException(nameof(userNameOrEmail));

            return this.usersRepository.FindAsync(UserOrEmailEqualsExpression(userNameOrEmail));
        }

        private Task StoreProfileImagesAsync(User user, IEnumerable<StorableImage> storeableImages)
        {
            var storeTasks = storeableImages.Select(async profileImageMetaInfo =>
            {
                await this.profileImageStorageService.StoreAsync(profileImageMetaInfo);

                user.AddProfileImage(profileImageMetaInfo.ProfileImage);
            });

            return Task.WhenAll(storeTasks);
        }

        private static Expression<Func<User, bool>> UserNameEqualsExpression(string userName)
        {
            return user => user.UserName.ToLower() == userName.ToLower();
        }

        private static Expression<Func<User, bool>> UserOrEmailEqualsExpression(string userNameOrEmail)
        {
            return user =>
                user.UserName.ToLower() == userNameOrEmail.ToLower() ||
                user.Emails.Any(email => email.Address.ToLower() == userNameOrEmail.ToLower());
        }
    }
}