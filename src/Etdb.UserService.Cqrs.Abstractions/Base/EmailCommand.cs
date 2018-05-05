using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class EmailCommand : IVoidCommand
    {
        public Guid Id { get; set; }
        
        public string Address { get; set; }

        public bool IsPrimary { get; set; }
    }
}