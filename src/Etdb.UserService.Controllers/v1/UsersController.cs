using System;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.General.Abstractions.Exceptions;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DataTransferObjects;
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
        [HttpGet]
        public async Task<UserDto> Get()
        {
            var user = await this.userRepository.GetAsync(new Guid("6aeb9030-90d4-4f51-afb5-a60d16e98761"));
            return this.mapper.Map<UserDto>(user);
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<UserDto> Registration([FromBody] UserRegisterDto userRegisterDTO)
        {
            if (!this.ModelState.IsValid)
            {
                throw new ModelStateValidationException("Register request is invalid!", this.ModelState);
            }

            var registerCommand = this.mapper.Map<UserRegisterCommand>(userRegisterDTO);
            var user = await this.mediator.SendCommandAsync<UserRegisterCommand, UserDto>(registerCommand);
            return user;
        }

        [AllowAnonymous]
        [HttpPut("update")]
        public async Task<UserDto> Update([FromBody] UserDto userDTO)
        {
            if (!this.ModelState.IsValid)
            {
                throw new ModelStateValidationException("Update request is invalid!", this.ModelState);
            }
            var updateCommand = this.mapper.Map<UserUpdateCommand>(userDTO);
            var user = await this.mediator.SendCommandAsync<UserUpdateCommand, UserDto>(updateCommand);
            return user;
        }
    }
}
