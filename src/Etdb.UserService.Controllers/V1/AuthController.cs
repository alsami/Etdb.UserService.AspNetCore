using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Controllers.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;
using Etdb.UserService.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUsersRepository repository;
        private readonly IMediatorHandler mediator;

        public AuthController(IUsersRepository repository, IMediatorHandler mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<UserDto> Registration([FromBody] UserRegisterCommand command)
        {
            if (!this.ModelState.IsValid)
            {
                this.ModelState.ThrowValidationError("User register command not valid");
            }
            
            var user = await this.mediator.SendCommandAsync<UserRegisterCommand, UserDto>(command);

            return user;
        }
    }
}