using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;
using MediatR;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class
        UserSearchByUsernameAndEmailCommandHandler : IRequestHandler<UserSearchByUsernameAndEmailCommand,
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