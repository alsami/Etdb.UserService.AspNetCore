using AutoMapper;
using ETDB.API.ServiceBase.Domain.Abstractions.Bus;
using ETDB.API.UserService.Application.Services;
using ETDB.API.UserService.Domain;
using ETDB.API.UserService.EventSourcing.Commands;
using ETDB.API.UserService.Presentation.DTO;

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
            var registerCommand = this.mapper.Map<UserRegisterCommand>(registerUserDTO);
            this.bus.SendCommand(registerCommand);
        }
    }
}
