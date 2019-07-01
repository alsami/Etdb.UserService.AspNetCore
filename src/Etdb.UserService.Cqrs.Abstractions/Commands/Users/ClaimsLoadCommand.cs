using System;
using System.Collections.Generic;
using System.Security.Claims;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class ClaimsLoadCommand : IResponseCommand<IEnumerable<Claim>>
    {
        public Guid Id { get; }

        public ClaimsLoadCommand(Guid id)
        {
            this.Id = id;
        }
    }
}