using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class UserNameCommand : IVoidCommand
    {
        public Guid Id { get; set; }
        
        public string UserName { get; set; }
    }
}