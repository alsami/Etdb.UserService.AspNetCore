using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.AuthenticationLogs
{
    public class AuthenticationLogsCleanupCommand : IVoidCommand
    {
        public TimeSpan LogsOlderThanSpan { get; }

        public AuthenticationLogsCleanupCommand(TimeSpan logsOlderThanSpan)
        {
            this.LogsOlderThanSpan = logsOlderThanSpan;
        }
    }
}