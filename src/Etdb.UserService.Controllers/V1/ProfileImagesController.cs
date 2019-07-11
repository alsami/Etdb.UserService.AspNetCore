using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.AspNetCore.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Presentation.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Etdb.UserService.Controllers.V1
{
    [Route("api/v1/users/{userId:Guid}/profileimages")]
    public class ProfileImagesController : Controller
    {
        private readonly IBus bus;

        public ProfileImagesController(IBus bus)
        {
            this.bus = bus;
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}", Name = RouteNames.ProfileImages.LoadRoute)]
        public async Task<IActionResult> LoadAsync(CancellationToken cancellationToken, Guid id,
            Guid userId)
        {
            var profileImageDownloadInfo =
                await this.bus.SendCommandAsync<ProfileImageLoadCommand, FileDownloadInfoDto>(
                    new ProfileImageLoadCommand(id, userId), cancellationToken);

            return new FileContentResult(profileImageDownloadInfo.File,
                new MediaTypeHeaderValue(profileImageDownloadInfo.MediaType))
            {
                FileDownloadName = profileImageDownloadInfo.Name,
                EnableRangeProcessing = true
            };
        }
        
        [AllowAnonymous]
        [HttpGet("{id:Guid}/resize", Name = RouteNames.ProfileImages.LoadResizedRoute)]
        public async Task<IActionResult> LoadThumbnailAsync(CancellationToken cancellationToken, Guid id,
            Guid userId, int dimensionX = 1024, int dimensionY = 1024)
        {
            var profileImageDownloadInfo =
                await this.bus.SendCommandAsync<ProfileImageResizedLoadCommand, FileDownloadInfoDto>(
                    new ProfileImageResizedLoadCommand(id, userId, dimensionX, dimensionY), cancellationToken);

            return new FileContentResult(profileImageDownloadInfo.File,
                new MediaTypeHeaderValue(profileImageDownloadInfo.MediaType))
            {
                FileDownloadName = profileImageDownloadInfo.Name,
                EnableRangeProcessing = true
            };
        }

        [HttpPost]
        public async Task<ProfileImageMetaInfoDto> UploadAsync(CancellationToken cancellationToken, Guid userId,
            [FromForm] IFormFile file)
        {
            var command = new ProfileImageAddCommand(userId,
                file.FileName,
                new ContentType(file.ContentType), await file.ReadFileBytesAsync());

            var user = await this.bus.SendCommandAsync<ProfileImageAddCommand, ProfileImageMetaInfoDto>(command,
                cancellationToken);

            return user;
        }

        [HttpPost("multiple")]
        public async Task<IEnumerable<ProfileImageMetaInfoDto>> UploadAsync(CancellationToken cancellationToken,
            Guid userId, [FromForm] IEnumerable<IFormFile> files)
        {
            var extractTasks = files.Select(async file => new UploadImageMetaInfo(file.FileName,
                new ContentType(file.ContentType), await file.ReadFileBytesAsync()));

            var uploadImageMetaInfos = await Task.WhenAll(extractTasks);

            var command = new ProfileImagesAddCommand(userId, uploadImageMetaInfos);

            return await this.bus.SendCommandAsync<ProfileImagesAddCommand, IEnumerable<ProfileImageMetaInfoDto>>(
                command, cancellationToken);
        }

        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> PrimaryImageAsync(CancellationToken cancellationToken, Guid userId, Guid id)
        {
            var command = new ProfileImageSetPrimaryCommand(id, userId);

            await this.bus.SendCommandAsync(command, cancellationToken);

            return this.NoContent();
        }

        [HttpDelete("{id:Guid}", Name = RouteNames.ProfileImages.DeleteRoute)]
        public async Task<IActionResult> RemoveAsync(CancellationToken cancellationToken, Guid userId,
            Guid id)
        {
            var command = new ProfileImageRemoveCommand(userId, id);

            await this.bus.SendCommandAsync(command, cancellationToken);

            return this.NoContent();
        }
    }
}