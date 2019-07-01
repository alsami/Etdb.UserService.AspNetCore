using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands.AuthenticationLogs;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.CommandHandler.AuthenticationLogs
{
    public class AuthenticationLogsForUserLoadCommandHandler : IResponseCommandHandler<AuthenticationLogsForUserLoadCommand, IEnumerable<AuthenticationLogDto>>
    {
        private readonly IMapper mapper;
        private readonly IUsersService usersService;

        public AuthenticationLogsForUserLoadCommandHandler(IMapper mapper, IUsersService usersService)
        {
            this.mapper = mapper;
            this.usersService = usersService;
        }

        public async Task<IEnumerable<AuthenticationLogDto>> Handle(AuthenticationLogsForUserLoadCommand command, CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(command.UserId);

            if (user == null) throw WellknownExceptions.UserNotFoundException();

            return this.mapper.Map<IEnumerable<AuthenticationLogDto>>(user.AuthenticationLogs);
        }
    }
}