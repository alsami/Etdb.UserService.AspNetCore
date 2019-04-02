using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class
        UserSearchByUsernameAndEmailCommandHandler : IResponseCommandHandler<UserSearchByUsernameAndEmailCommand,
            UserDto>
    {
        private readonly IUsersService usersService;
        private readonly IMapper mapper;

        public UserSearchByUsernameAndEmailCommandHandler(IUsersService usersService, IMapper mapper)
        {
            this.usersService = usersService;
            this.mapper = mapper;
        }

        public async Task<UserDto> Handle(UserSearchByUsernameAndEmailCommand command,
            CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByUserNameOrEmailAsync(command.UserNameOrEmail);

            return this.mapper.Map<UserDto>(user);
        }
    }
}