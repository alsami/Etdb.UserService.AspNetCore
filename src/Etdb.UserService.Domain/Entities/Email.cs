using System;
using Etdb.UserService.Domain.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Etdb.UserService.Domain.Entities
{
    public class Email : GuidDocument
    {
        public Email(Guid id, Guid userId, string address, bool isPrimary, bool isExternal) : base(id)
        {
            this.Address = address;
            this.IsPrimary = isPrimary;
            this.IsExternal = isExternal;
        }

        public Guid UserId { get; }

        public string Address { get; private set; }

        public bool IsPrimary { get; private set; }

        public bool IsExternal { get; private set; }
    }
}