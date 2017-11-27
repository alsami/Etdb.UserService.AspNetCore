using System;
using AutoMapper;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Base;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Bus;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Handler;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Notifications;
using ETDB.API.ServiceBase.Repositories.Abstractions.Generics;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.EventSourcing.Commands;
using ETDB.API.UserService.Presentation.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETDB.API.UserService.Controller.v1
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
                .GetIncluding(id, user => user.UserSecurityroles);

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
