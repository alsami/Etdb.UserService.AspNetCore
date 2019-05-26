using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.AspNetCore.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
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
        [HttpGet("{id:Guid}", Name = RouteNames.ProfileImages.ProfileImageLoadRoute)]
        public async Task<IActionResult> ProfileImageLoadAsync(CancellationToken cancellationToken, Guid id,
            Guid userId)
        {
            var profileImageDownloadInfo =
                await this.bus.SendCommandAsync<ProfileImageLoadCommand, FileDownloadInfoDto>(
                    new ProfileImageLoadCommand(id, userId), cancellationToken);

            return new FileContentResult(profileImageDownloadInfo.File,
                new MediaTypeHeaderValue(profileImageDownloadInfo.MediaType))
            {
                FileDownloadName = profileImageDownloadInfo.Name
            };
        }

        [HttpPatch]
        public async Task<UserDto> ProfileImageUploadAsync(CancellationToken cancellationToken, Guid userId,
            [FromForm] IFormFile file)
        {
            var command = new ProfileImageAddCommand(userId,
                file.FileName,
                new ContentType(file.ContentType), await file.ReadFileBytesAsync());

            var user = await this.bus.SendCommandAsync<ProfileImageAddCommand, UserDto>(command, cancellationToken);

            return user;
        }

        [HttpDelete("{id:Guid}", Name = RouteNames.ProfileImages.ProfileImageDeleteRoute)]
        public async Task<IActionResult> ProfileImageRemoveAsync(CancellationToken cancellationToken, Guid userId,
            Guid id)
        {
            var command = new ProfileImageRemoveCommand(userId, id);

            await this.bus.SendCommandAsync(command, cancellationToken);

            return this.NoContent();
        }
    }
}