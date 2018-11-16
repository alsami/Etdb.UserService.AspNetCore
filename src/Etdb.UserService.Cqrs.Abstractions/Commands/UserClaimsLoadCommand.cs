using System;
using System.Collections.Generic;
using System.Security.Claims;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserClaimsLoadCommand : IResponseCommand<IEnumerable<Claim>>
    {
        public Guid Id { get; }

        public UserClaimsLoadCommand(Guid id)
        {
            this.Id = id;
        }
    }
}