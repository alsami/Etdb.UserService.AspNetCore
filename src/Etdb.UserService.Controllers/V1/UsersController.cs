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
        private readonly IMediatorHandler mediator;

        public UsersController(IMediatorHandler mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        public async Task<UserDto> GetUser(Guid id)
        {
            var command = new UserLoadCommand(id);

            var user = await this.mediator.SendCommandAsync<UserLoadCommand, UserDto>(command);

            return user;
        }

        [HttpPatch("{id:Guid}/profileimage")]
        public async Task<UserDto> UploadProfileImage(Guid id, [FromForm] IFormFile file)
        {
            var command = new UserProfileImageAddCommand(id,
                file.FileName,
                new ContentType(file.ContentType), await file.ReadFileBytesAsync());
            
            var user = await this.mediator.SendCommandAsync<UserProfileImageAddCommand, UserDto>(command);

            return user;
        }

        [AllowAnonymous]
        [ResponseCache(NoStore = true)]
        [HttpGet("{id:Guid}/profileimage", Name = RouteNames.UserProfileImageUrlRoute)]
        public async Task<IActionResult> GetUserProfileImage(Guid id)
        {
  
            var fileInfo =
                await this.mediator.SendCommandAsync<UserProfileImageLoadCommand, FileInfo>(
                    new UserProfileImageLoadCommand(id));
            
            return new FileContentResult(fileInfo.File, new MediaTypeHeaderValue(fileInfo.MediaType))
            {
                FileDownloadName = fileInfo.Name
            };
        }
    }
}