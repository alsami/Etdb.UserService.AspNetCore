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

namespace Etdb.UserService.Controller.v1
{
    [Route("api/v1/[controller]")]
    public class UsersController : EventSourcingController
    {
        private readonly IMapper mapper;
        private readonly IMediatorHandler mediator;
        private readonly IEntityRepository<User> userBaseRepository;

        public UsersController(IMapper mapper,
            IMediatorHandler mediator,
            IDomainNotificationHandler<DomainNotification> notifications,
            IEntityRepository<User> userBaseRepository) : base(notifications)
        {
            this.mapper = mapper;
            this.mediator = mediator;
            this.userBaseRepository = userBaseRepository;
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        public UserDTO GetUser(Guid id)
        {
            var requestedUser = this.userBaseRepository
                .Get(id, user => user.UserSecurityroles);

            if (requestedUser == null)
            {
                // TODO
                throw new Exception("");
            }

            return this.mapper.Map<UserDTO>(requestedUser);
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public IActionResult Registration([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var registerCommand = this.mapper.Map<UserRegisterCommand>(userRegisterDTO);
            this.mediator.SendCommand(registerCommand);
            return this.ExecuteResult();
        }
    }
}
