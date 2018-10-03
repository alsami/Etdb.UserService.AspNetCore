using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class EmailCommand : IVoidCommand
    {
        public Guid Id { get; }

        public string Address { get; }

        public bool IsPrimary { get; }

        protected EmailCommand(Guid id, string address, bool isPrimary)
        {
            this.Id = id;
            this.Address = address;
            this.IsPrimary = isPrimary;
        }

        protected EmailCommand(string address, bool isPrimary)
        {
            Address = address;
            IsPrimary = isPrimary;
        }
    }
}