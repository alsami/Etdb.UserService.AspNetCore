using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.ServiceBase.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IBus bus;
        private readonly IMapper mapper;

        public AuthController(IBus bus, IMapper mapper)
        {
            this.bus = bus;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegisterDto dto)
        {
            if (!this.ModelState.IsValid)
                throw this.ModelState.GenerateValidationException("User register request is invalid!");

            var command = this.mapper.Map<UserRegisterCommand>(dto);

            await this.bus.SendCommandAsync(command);

            return this.NoContent();
        }
    }
}