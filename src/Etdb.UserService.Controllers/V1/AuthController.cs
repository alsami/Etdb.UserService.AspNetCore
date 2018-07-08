using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.ServiceBase.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IBus bus;

        public AuthController(IBus bus)
        {
            this.bus = bus;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegisterCommand command)
        {
            if (!this.ModelState.IsValid)
            {
                throw this.ModelState.GenerateValidationException("User register command is invalid!");
            }
            
            await this.bus.SendCommandAsync(command);

            return this.NoContent();
        }
    }
}