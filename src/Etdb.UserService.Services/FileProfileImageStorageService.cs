using System;
using System.IO;
using System.Threading.Tasks;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.ValueObjects;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Services.Abstractions;
using Etdb.UserService.Services.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Services
{
    public class FileProfileImageStorageService : IProfileImageStorageService
    {
        private readonly IFileService fileService;
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;
        private readonly IImageCompressionService imageCompressionService;
        private readonly ILogger<UsersService> logger;

        public FileProfileImageStorageService(IFileService fileService, IOptions<FilestoreConfiguration> fileStoreOptions, IImageCompressionService imageCompressionService, ILogger<UsersService> logger)
        {
            this.fileService = fileService;
            this.fileStoreOptions = fileStoreOptions;
            this.imageCompressionService = imageCompressionService;
            this.logger = logger;
        }

        public async Task StoreAsync(StorableImage storableImage)
        {
            var relativePath = Path.Combine(this.fileStoreOptions.Value.ImagePath,
                storableImage.ProfileImage.UserId.ToString());

            await this.RemoveAsync(storableImage.ProfileImage);

            var mediaType = storableImage.ProfileImage.MediaType == "image/*"
                ? "image/jpeg"
                : storableImage.ProfileImage.MediaType;

            var compressionFactor = storableImage.Image.Length > 1024 * 10 ? 25L : 50L;

            this.logger.LogInformation("Compressing image with factory {compressionFactor}. Current size: {size}",
                compressionFactor, storableImage.Image.Length);

            var compressed =
                this.imageCompressionService.Compress(storableImage.Image.ToArray(), mediaType,
                    compressionFactor);

            this.logger.LogInformation("Compressing image done. Compressed size: {size}", compressed.Length);

            await this.fileService.StoreBinaryAsync(relativePath, storableImage.ProfileImage.Name,
                compressed.AsMemory());
        }

        public Task RemoveAsync(ProfileImage profileImage)
        {
            this.fileService.DeleteBinary(Path.Combine(this.fileStoreOptions.Value.ImagePath, profileImage.SubPath()),
                profileImage.Name);
            
            return Task.CompletedTask;
        }
    }
}