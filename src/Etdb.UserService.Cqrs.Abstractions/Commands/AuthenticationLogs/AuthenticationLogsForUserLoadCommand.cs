using System;
using System.Collections.Generic;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.AuthenticationLogs
{
    public class AuthenticationLogsForUserLoadCommand : IResponseCommand<IEnumerable<AuthenticationLogDto>>
    {
        public Guid UserId { get; }

        public AuthenticationLogsForUserLoadCommand(Guid userId)
        {
            this.UserId = userId;
        }
    }
}