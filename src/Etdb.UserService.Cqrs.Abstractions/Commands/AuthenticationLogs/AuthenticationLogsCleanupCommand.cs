using System;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.AuthenticationLogs
{
    public class AuthenticationLogsCleanupCommand : IRequest
    {
        public TimeSpan LogsOlderThanSpan { get; }

        public AuthenticationLogsCleanupCommand(TimeSpan logsOlderThanSpan)
        {
            this.LogsOlderThanSpan = logsOlderThanSpan;
        }
    }
}