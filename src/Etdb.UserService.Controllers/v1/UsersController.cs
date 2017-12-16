using System;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Base;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.EventSourcing.Abstractions.Handler;
using Etdb.ServiceBase.EventSourcing.Abstractions.Notifications;
using Etdb.ServiceBase.Repositories.Abstractions.Generics;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.v1
{
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IMediatorHandler mediator;

        public UsersController(IMapper mapper,
            IMediatorHandler mediator)
        {
            this.mapper = mapper;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public UserDTO Registration([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var registerCommand = this.mapper.Map<UserRegisterCommand>(userRegisterDTO);
            var task = this.mediator.SendCommand<UserRegisterCommand, UserDTO>(registerCommand);
            return task.Result;
        }
    }
}
