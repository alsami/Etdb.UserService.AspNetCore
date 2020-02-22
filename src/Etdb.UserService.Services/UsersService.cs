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
        private readonly IFileService fileService;
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;
        private readonly IImageCompressionService imageCompressionService;
        private readonly ILogger<UsersService> logger;
        private readonly IUsersRepository usersRepository;

        public UsersService(IUsersRepository usersRepository, IFileService fileService,
            IOptions<FilestoreConfiguration> fileStoreOptions, IImageCompressionService imageCompressionService,
            ILogger<UsersService> logger)
        {
            this.usersRepository = usersRepository;
            this.fileService = fileService;
            this.fileStoreOptions = fileStoreOptions;
            this.imageCompressionService = imageCompressionService;
            this.logger = logger;
        }

        public async Task AddAsync(User user, params StoreImageMetaInfo[] storeImageMetaInfos)
        {
            if (storeImageMetaInfos.Any()) await this.StoreProfileImagesAsync(user, storeImageMetaInfos);

            await this.usersRepository.AddAsync(user);
        }

        public async Task EditAsync(User user, params StoreImageMetaInfo[] storeImageMetaInfos)
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

        private Task StoreProfileImagesAsync(User user, IEnumerable<StoreImageMetaInfo> profileImageMetaInfos)
        {
            var storeTasks = profileImageMetaInfos.Select(async profileImageMetaInfo =>
            {
                var relativePath = Path.Combine(this.fileStoreOptions.Value.ImagePath,
                    profileImageMetaInfo.ProfileImage.SubPath());

                // TODO: fix path creation in file-service
                var absolutePath = Path.Combine(this.fileStoreOptions.Value.ImagePath,
                    profileImageMetaInfo.ProfileImage.RelativePath());

                this.fileService.DeleteBinary(absolutePath);

                var mediaType = profileImageMetaInfo.ProfileImage.MediaType == "image/*"
                    ? "image/jpeg"
                    : profileImageMetaInfo.ProfileImage.MediaType;

                var compressionFactor = profileImageMetaInfo.Image.Length > 1024 * 10 ? 25L : 50L;

                this.logger.LogInformation("Compressing image with factory {compressionFactor}. Current size: {size}",
                    compressionFactor, profileImageMetaInfo.Image.Length);

                var compressed =
                    this.imageCompressionService.Compress(profileImageMetaInfo.Image.ToArray(), mediaType,
                        compressionFactor);

                this.logger.LogInformation("Compressing image done. Compressed size: {size}", compressed.Length);

                await this.fileService.StoreBinaryAsync(relativePath, profileImageMetaInfo.ProfileImage.Name,
                    compressed.AsMemory());

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