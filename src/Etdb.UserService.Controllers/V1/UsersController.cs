using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Presentation.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator bus;

        public UsersController(IMediator bus) => this.bus = bus;

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        public Task<UserDto> LoadAsync(CancellationToken cancellationToken, Guid id)
        {
            var command = new UserLoadCommand(id);

            return this.bus.Send(command, cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet("search/{*userName}")]
        public Task<IEnumerable<UserFlatDto>> FindAsync(CancellationToken cancellationToken, string userName)
        {
            var command = new UsersSearchCommand(userName);

            return this.bus.Send(command, cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet("availability/{*userName}")]
        public Task<UserNameAvailabilityDto> AvailabilityAsync(CancellationToken cancellationToken,
            string userName)
        {
            var command = new UserNameAvailabilityCheckCommand(userName);

            return this.bus.Send(command,
                cancellationToken);
        }

        [HttpPatch("{id:Guid}/username/{*userName}")]
        public async Task<IActionResult> UserNameChangeAsync(Guid id,
            string userName)
        {
            var command = new UserNameChangeCommand(id, userName);

            await this.bus.Send(command);

            return this.NoContent();
        }


        [HttpPatch("{id:Guid}/password")]
        public async Task<IActionResult> PasswordChangeAsync(Guid id,
            [FromBody] UserPasswordChangeDto dto)
        {
            var command = new UserPasswordChangeCommand(id, dto.NewPassword, dto.CurrentPassword);

            await this.bus.Send(command);

            return this.NoContent();
        }

        [HttpPatch("{id:Guid}/profileinfo")]
        public async Task<IActionResult> ProfileInfoChangeAsync(Guid id,
            [FromBody] UserProfileInfoChangeDto dto)
        {
            var command = new UserProfileInfoChangeCommand(id, dto.FirstName, dto.Name, dto.Biography);

            await this.bus.Send(command);

            return this.NoContent();
        }
    }
}