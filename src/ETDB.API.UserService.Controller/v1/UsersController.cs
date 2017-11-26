using System;
using AutoMapper;
using ETDB.API.ServiceBase.Abstractions.Repositories;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Handler;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Notifications;
using ETDB.API.UserService.Application.Services;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Presentation.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETDB.API.UserService.Controller.v1
{
    [Route("api/v1/[controller]")]
    public class UsersController : ApiController
    {
        private readonly IMapper mapper;
        private readonly IUserAppService userAppService;
        private readonly IEntityRepository<User> userBaseRepository;

        public UsersController(IMapper mapper,
            IDomainNotificationHandler<DomainNotification> notifications, 
            IUserAppService userAppService, IEntityRepository<User> userBaseRepository) : base(notifications)
        {
            this.mapper = mapper;
            this.userAppService = userAppService;
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
            this.userAppService.Register(userRegisterDTO);
            return Response(NoContent());
        }
    }
}
