using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Controllers.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Etdb.UserService.Controllers.V1
{
    [Route("api/v1/[controller]", Name = ControllerNames.UsersController)]
    public class UsersController : Controller
    {
        private readonly IBus bus;

        public UsersController(IBus bus)
        {
            this.bus = bus;
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        public Task<UserDto> LoadAsync(CancellationToken cancellationToken, Guid id)
        {
            var command = new UserLoadCommand(id);

            return this.bus.SendCommandAsync<UserLoadCommand, UserDto>(command, cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}/profileimage", Name = RouteNames.UserProfileImageUrlRoute)]
        public async Task<IActionResult> ProfileImageLoadAsync(CancellationToken cancellationToken, Guid id)
        {
            var fileInfo =
                await this.bus.SendCommandAsync<UserProfileImageLoadCommand, FileDownloadInfoDto>(
                    new UserProfileImageLoadCommand(id), cancellationToken);

            return new FileContentResult(fileInfo.File, new MediaTypeHeaderValue(fileInfo.MediaType))
            {
                FileDownloadName = fileInfo.Name
            };
        }

        [HttpPatch("{id:Guid}/profileinfo")]
        public async Task<IActionResult> ProfileInfoChangeAsync(CancellationToken cancellationToken, Guid id,
            [FromBody] UserProfileInfoChangeDto dto)
        {
            var command = new UserProfileInfoChangeCommand(id, dto.FirstName, dto.Name, dto.Biography);

            await this.bus.SendCommandAsync(command, cancellationToken);

            return this.NoContent();
        }

        [HttpPatch("{id:Guid}/password")]
        public async Task<IActionResult> PasswordChangeAsync(CancellationToken cancellationToken, Guid id,
            [FromBody] UserPasswordChangeDto dto)
        {
            var command = new UserPasswordChangeCommand(id, dto.NewPassword, dto.CurrentPassword);

            await this.bus.SendCommandAsync(command, cancellationToken);

            return this.NoContent();
        }

        [HttpPatch("{id:Guid}/profileimage")]
        public async Task<UserDto> ProfileImageUploadAsync(CancellationToken cancellationToken, Guid id,
            [FromForm] IFormFile file)
        {
            var command = new UserProfileImageAddCommand(id,
                file.FileName,
                new ContentType(file.ContentType), await file.ReadFileBytesAsync());

            var user = await this.bus.SendCommandAsync<UserProfileImageAddCommand, UserDto>(command, cancellationToken);

            return user;
        }

        [HttpDelete("{id:Guid}/profileimage")]
        public async Task<IActionResult> ProfileImageRemoveAsync(CancellationToken cancellationToken, Guid id)
        {
            var command = new UserProfileImageRemoveCommand(id);

            await this.bus.SendCommandAsync(command, cancellationToken);

            return this.NoContent();
        }
    }
}