using System;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Presentation.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public Task<UserDto> LoadAsync(CancellationToken cancellationToken, Guid id)
        {
            var command = new UserLoadCommand(id);

            return this.bus.SendCommandAsync<UserLoadCommand, UserDto>(command, cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet("availability/{*userName}")]
        public Task<UserNameAvailabilityDto> AvailabilityAsync(CancellationToken cancellationToken,
            string userName)
        {
            var command = new UserNameAvailabilityCheckCommand(userName);

            return this.bus.SendCommandAsync<UserNameAvailabilityCheckCommand, UserNameAvailabilityDto>(command, cancellationToken);
        }

        [HttpPatch("{id:Guid}/username/{userName}")]
        public async Task<IActionResult> UserNameChangeAsync(CancellationToken cancellationToken, Guid id, string userName)
        {
            var command = new UserNameChangeCommand(id, userName);

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

        [HttpPatch("{id:Guid}/profileinfo")]
        public async Task<IActionResult> ProfileInfoChangeAsync(CancellationToken cancellationToken, Guid id,
            [FromBody] UserProfileInfoChangeDto dto)
        {
            var command = new UserProfileInfoChangeCommand(id, dto.FirstName, dto.Name, dto.Biography);

            await this.bus.SendCommandAsync(command, cancellationToken);

            return this.NoContent();
        }
    }
}