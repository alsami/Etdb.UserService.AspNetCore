using System;
using System.Collections.Generic;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class AuthenticationLogsLoadCommand : IResponseCommand<IEnumerable<AuthenticationLogDto>>
    {
        public Guid UserId { get; }

        public AuthenticationLogsLoadCommand(Guid userId)
        {
            this.UserId = userId;
        }
    }
}