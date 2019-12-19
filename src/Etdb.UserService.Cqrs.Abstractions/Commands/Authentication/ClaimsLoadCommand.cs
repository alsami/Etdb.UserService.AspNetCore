using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Authentication
{
    public class ClaimsLoadCommand : IRequest<IEnumerable<Claim>>
    {
        public Guid Id { get; }

        public ClaimsLoadCommand(Guid id)
        {
            this.Id = id;
        }
    }
}