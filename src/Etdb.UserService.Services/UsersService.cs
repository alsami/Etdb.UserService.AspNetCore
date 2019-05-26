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
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Services
{
    // ReSharper disable SpecifyStringComparison
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository;
        private readonly IFileService fileService;
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;
        private const int MaxFailedLoginCount = 3;

        public UsersService(IUsersRepository usersRepository, IFileService fileService,
            IOptions<FilestoreConfiguration> fileStoreOptions)
        {
            this.usersRepository = usersRepository;
            this.fileService = fileService;
            this.fileStoreOptions = fileStoreOptions;
        }

        public async Task AddAsync(User user, params ProfileImageMetaInfo[] profileImageMetaInfos)
        {
            if (profileImageMetaInfos.Any())
            {
                await this.StoreProfileImagesAsync(user, profileImageMetaInfos);
            }

            await this.usersRepository.AddAsync(user);
        }

        public async Task EditAsync(User user, params ProfileImageMetaInfo[] profileImageMetaInfos)
        {
            if (profileImageMetaInfos.Any())
            {
                await this.StoreProfileImagesAsync(user, profileImageMetaInfos);
            }

            await this.usersRepository.EditAsync(user);
        }

        public Task EditAsync(User user) => this.usersRepository.EditAsync(user);

        public Task<User> FindByIdAsync(Guid id) => this.usersRepository.FindAsync(id);

        public async Task<bool> IsUserLocked(Guid id)
        {
            var user = await this.usersRepository.FindAsync(id);

            if (user.AuthenticationProvider != AuthenticationProvider.UsernamePassword)
            {
                return false;
            }

            if (user.SignInLogs == null)
            {
                return false;
            }

            var signInLogs = user.SignInLogs
                .OrderByDescending(log => log.LoggedAt)
                .ToArray();

            if (signInLogs.Length < UsersService.MaxFailedLoginCount ||
                signInLogs.FirstOrDefault(signInLog => signInLog.SignInType == SignInType.Succeeded) != null)
            {
                return false;
            }

            var foundFailedLoginsInARow = 0;

            foreach (var signInLog in signInLogs)
            {
                if (signInLog.SignInType != SignInType.Failed)
                {
                    foundFailedLoginsInARow = 0;
                    continue;
                }

                foundFailedLoginsInARow++;

                if (foundFailedLoginsInARow == UsersService.MaxFailedLoginCount) break;
            }

            return foundFailedLoginsInARow == UsersService.MaxFailedLoginCount;
        }

        public Task<User> FindByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException(nameof(userName));

            return this.usersRepository.FindAsync(UserNameEqualsExpression(userName));
        }

        public Task<User> FindByUserNameOrEmailAsync(string userNameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(userNameOrEmail)) throw new ArgumentException(nameof(userNameOrEmail));

            return this.usersRepository.FindAsync(UserOrEmailEqualsExpression(userNameOrEmail));
        }

        public Email FindEmailAddress(string emailAddress)
        {
            var email = this.usersRepository
                .Query()
                .SelectMany(user => user.Emails)
                .FirstOrDefault(EmailEqualsExpression(emailAddress));

            return email;
        }

        private Task StoreProfileImagesAsync(User user, IEnumerable<ProfileImageMetaInfo> profileImageMetaInfos)
        {
            var storeTasks = profileImageMetaInfos.Select(async profileImageMetaInfo =>
            {
                var relativePath = Path.Combine(this.fileStoreOptions.Value.ImagePath,
                    profileImageMetaInfo.ProfileImage.Subpath());
                
                // TODO: fix path creation in file-service
                var absolutePath = Path.Combine(this.fileStoreOptions.Value.ImagePath, profileImageMetaInfo.ProfileImage.RelativePath());
                
                this.fileService.DeleteBinary(absolutePath);
                
                await this.fileService.StoreBinaryAsync(relativePath, profileImageMetaInfo.ProfileImage.Name, profileImageMetaInfo.Image);

                user.AddProfileImage(profileImageMetaInfo.ProfileImage);
            });

            return Task.WhenAll(storeTasks);
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