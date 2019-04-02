using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class EmailCommand : IVoidCommand
    {
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public Guid Id { get; }

        public string Address { get; }

        public bool IsPrimary { get; }

        public bool IsExternal { get; }

        protected EmailCommand(Guid id, string address, bool isPrimary, bool isExternal)
        {
            this.Id = id;
            this.Address = address;
            this.IsPrimary = isPrimary;
            this.IsExternal = isExternal;
        }
    }
}