using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Constants;
using Etdb.UserService.Controllers.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Abstractions.Models;
using Etdb.UserService.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Etdb.UserService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IBus bus;

        public UsersController(IBus bus)
        {
            this.bus = bus;
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        public async Task<UserDto> LoadAsync(Guid id)
        {
            var command = new UserLoadCommand(id);

            var user = await this.bus.SendCommandAsync<UserLoadCommand, UserDto>(command);

            return user;
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}/profileimage", Name = RouteNames.UserProfileImageUrlRoute)]
        public async Task<IActionResult> ProfileImageLoadAsync(Guid id)
        {
            var fileInfo =
                await this.bus.SendCommandAsync<UserProfileImageLoadCommand, FileDownloadInfo>(
                    new UserProfileImageLoadCommand(id));

            return new FileContentResult(fileInfo.File, new MediaTypeHeaderValue(fileInfo.MediaType)) {
                FileDownloadName = fileInfo.Name
            };
        }

        [HttpPatch("{id:Guid}/profileinfo")]
        public async Task<IActionResult> ProfileInfoChangeAsync(Guid id, [FromBody] UserProfileInfoChangeDto dto)
        {
            var command = new UserProfileInfoChangeCommand(id, dto.FirstName, dto.Name, dto.Biography);

            await this.bus.SendCommandAsync(command);

            return this.NoContent();
        }

        [HttpPatch("{id:Guid}/password")]
        public async Task<IActionResult> PasswordChangeAsync(Guid id, [FromBody] UserPasswordChangeDto dto)
        {
            var command = new UserPasswordChangeCommand(id, dto.NewPassword, dto.CurrentPassword);

            await this.bus.SendCommandAsync(command);

            return this.NoContent();
        }

        [HttpPatch("{id:Guid}/profileimage")]
        public async Task<UserDto> ProfileImageUploadAsync(Guid id, [FromForm] IFormFile file)
        {
            var command = new UserProfileImageAddCommand(id,
                file.FileName,
                new ContentType(file.ContentType), await file.ReadFileBytesAsync());

            var user = await this.bus.SendCommandAsync<UserProfileImageAddCommand, UserDto>(command);

            return user;
        }

        [HttpDelete("{id:Guid}/profileimage")]
        public async Task<IActionResult> ProfileImageRemoveAsync(Guid id)
        {
            var command = new UserProfileImageRemoveCommand(id);

            await this.bus.SendCommandAsync(command);

            return NoContent();
        }
    }
}