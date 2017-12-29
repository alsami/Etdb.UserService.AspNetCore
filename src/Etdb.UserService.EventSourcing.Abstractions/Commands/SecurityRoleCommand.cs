using System.Collections.Generic;
using Etdb.ServiceBase.EventSourcing.Abstractions.Commands;

namespace Etdb.UserService.EventSourcing.Abstractions.Commands
{
    public abstract class SecurityRoleCommand<TResponse> : TransactionCommand<TResponse> where TResponse : class
    {
        public string Id { get; set; }

        public byte[] RowVersion { get; set; }

        public string Description { get; set; }
    }
}
