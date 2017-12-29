using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.General.Abstractions.Exceptions;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.v1
{
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IMediatorHandler mediator;

        public UsersController(IMapper mapper, IMediatorHandler mediator)
        {
            this.mapper = mapper;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<UserDto> Registration([FromBody] UserRegisterDto userRegisterDto)
        {
            if (!this.ModelState.IsValid)
            {
                throw new ModelStateValidationException("Register request is invalid!", this.ModelState);
            }

            var registerCommand = this.mapper.Map<UserRegisterCommand>(userRegisterDto);
            var user = await this.mediator.SendCommandAsync<UserRegisterCommand, UserDto>(registerCommand);
            return user;
        }

        [AllowAnonymous]
        [HttpPut("update")]
        public async Task<UserDto> Update([FromBody] UserDto userDto)
        {
            if (!this.ModelState.IsValid)
            {
                throw new ModelStateValidationException("Update request is invalid!", this.ModelState);
            }
            var updateCommand = this.mapper.Map<UserUpdateCommand>(userDto);
            var user = await this.mediator.SendCommandAsync<UserUpdateCommand, UserDto>(updateCommand);
            return user;
        }
    }
}
