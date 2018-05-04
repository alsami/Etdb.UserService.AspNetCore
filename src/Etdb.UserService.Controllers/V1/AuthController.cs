using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.ServiceBase.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository repository;
        private readonly IMediatorHandler mediator;

        public AuthController(IUserRepository repository, IMediatorHandler mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegisterCommand command)
        {
            if (!this.ModelState.IsValid)
            {
                throw this.ModelState.GenerateValidationException("User register command is invalid!");
            }
            
            await this.mediator.SendCommandAsync(command);

            return this.NoContent();
        }
    }
}