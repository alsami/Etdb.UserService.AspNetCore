using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Presentation.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    [Route("api/v1/users/{userId:Guid}/authentication-logs")]
    public class AuthenticationLogsController : Controller
    {
        private readonly IBus bus;

        public AuthenticationLogsController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpGet(Name = RouteNames.AuthenticationLogs.LoadAllRoute)]
        public Task<IEnumerable<AuthenticationLogDto>> LoadAsync(CancellationToken cancellationToken, Guid userId)
        {
            var command = new AuthenticationLogsLoadCommand(userId);

            return this.bus.SendCommandAsync<AuthenticationLogsLoadCommand, IEnumerable<AuthenticationLogDto>>(command,
                cancellationToken);
        }
    }
}