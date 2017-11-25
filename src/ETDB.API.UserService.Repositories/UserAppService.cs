using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ETDB.API.ServiceBase.Domain.Abstractions.Bus;
using ETDB.API.UserService.Domain;
using ETDB.API.UserService.Domain.Commands;
using ETDB.API.UserService.Domain.DTO;

namespace ETDB.API.UserService.Repositories
{
    public class UserAppService : IUserAppService
    {
        private readonly IMapper mapper;
        private readonly IMediatorHandler bus;

        public UserAppService(IMapper mapper, IMediatorHandler bus)
        {
            this.mapper = mapper;
            this.bus = bus;
        }

        public void Register(RegisterUserDTO registerUserDTO)
        {
            var registerCommand = this.mapper.Map<RegisterUserCommand>(registerUserDTO);
            this.bus.SendCommand(registerCommand);
        }
    }
}
