using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.AuthenticationLogs;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Presentation.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    [ApiController]
    [Route("api/v1/users/{userId:Guid}/authentication-logs")]
    public class AuthenticationLogsController : ControllerBase
    {
        private readonly IMediator bus;

        public AuthenticationLogsController(IMediator bus)
        {
            this.bus = bus;
        }

        [HttpGet(Name = RouteNames.AuthenticationLogs.LoadAllRoute)]
        public Task<IEnumerable<AuthenticationLogDto>> LoadAsync(CancellationToken cancellationToken, Guid userId)
        {
            var command = new AuthenticationLogsForUserLoadCommand(userId);

            return this.bus.Send(
                command,
                cancellationToken);
        }
    }
}