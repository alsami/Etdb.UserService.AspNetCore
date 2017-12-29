using System;
using Etdb.ServiceBase.EventSourcing.Abstractions.Events;

namespace Etdb.UserService.EventSourcing.Abstractions.Events
{
    public abstract class SecurityRoleEvent : Event
    {
        public Guid Id { get; set; }

        public byte[] RowVersion { get; set; }

        public string Designation { get; set; }

        public string Description { get; set; }

        public bool IsSystem { get; set; }
    }
}
