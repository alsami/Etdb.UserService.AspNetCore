using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class EmailCommand : IVoidCommand
    {
        protected EmailCommand(Guid id, string address, bool isPrimary)
        {
            this.Id = id;
            this.Address = address;
            this.IsPrimary = isPrimary;
        }

        protected EmailCommand(string address, bool isPrimary)
        {
            this.Address = address;
            this.IsPrimary = isPrimary;
        }

        public Guid Id { get; }

        public string Address { get; }

        public bool IsPrimary { get; }
    }
}