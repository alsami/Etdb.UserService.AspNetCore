using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.General.Abstractions.Exceptions;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;
using Etdb.UserService.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.v1
{
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IMediatorHandler mediator;
        private readonly IUserRepository userRepository;

        public UsersController(IMapper mapper,
            IMediatorHandler mediator, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.mediator = mediator;
            this.userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<UserDTO> Registration([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (!this.ModelState.IsValid)
            {
                throw new ModelStateValidationException("Register request is invalid!", this.ModelState);
            }

            var registerCommand = this.mapper.Map<UserRegisterCommand>(userRegisterDTO);
            var user = await this.mediator.SendCommandAsync<UserRegisterCommand, UserDTO>(registerCommand);
            return user;
        }

        [AllowAnonymous]
        [HttpPut("update")]
        public async Task<UserDTO> Update([FromBody] UserDTO userDTO)
        {
            if (!this.ModelState.IsValid)
            {
                throw new ModelStateValidationException("Update request is invalid!", this.ModelState);
            }
            var updateCommand = this.mapper.Map<UserUpdateCommand>(userDTO);
            var user = await this.mediator.SendCommandAsync<UserUpdateCommand, UserDTO>(updateCommand);
            return user;
        }
    }
}
