using System;
using System.Collections.Generic;
using Etdb.UserService.Presentation.Authentication;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.AuthenticationLogs
{
    public class AuthenticationLogsForUserLoadCommand : IRequest<IEnumerable<AuthenticationLogDto>>
    {
        public Guid UserId { get; }

        public AuthenticationLogsForUserLoadCommand(Guid userId)
        {
            this.UserId = userId;
        }
    }
}