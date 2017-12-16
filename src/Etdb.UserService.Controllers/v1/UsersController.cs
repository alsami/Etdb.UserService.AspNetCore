using System;
using System.Linq;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Base;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.EventSourcing.Abstractions.Handler;
using Etdb.ServiceBase.EventSourcing.Abstractions.Notifications;
using Etdb.ServiceBase.General.Abstractions.Exceptions;
using Etdb.ServiceBase.Repositories.Abstractions.Generics;
using Etdb.UserService.Domain.Entities;
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
        public UserDTO Registration([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (!this.ModelState.IsValid)
            {
                throw new ModelStateValidationException("Register request is invalid!", this.ModelState.Values
                    .SelectMany(value => value.Errors.Select(error => error.ErrorMessage)).ToArray());
            }

            var registerCommand = this.mapper.Map<UserRegisterCommand>(userRegisterDTO);
            var task = this.mediator.SendCommand<UserRegisterCommand, UserDTO>(registerCommand);
            return task.Result;
        }

        [AllowAnonymous]
        [HttpPut]
        public UserDTO Update([FromBody] UserDTO userDTO)
        {
            var existingUser = this.userRepository.Get(userDTO.Id);

            if (userDTO.ConccurencyToken != existingUser.RowVersion)
            {
                throw new ConcurrencyException("", this.mapper.Map<UserDTO>(existingUser));
            }

            this.mapper.Map(userDTO, existingUser);
            this.userRepository.Edit(existingUser);
            this.userRepository.EnsureChanges();
            return this.mapper.Map<UserDTO>(existingUser);
        }
    }
}
