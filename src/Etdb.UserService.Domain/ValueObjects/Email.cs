using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Etdb.UserService.Domain.ValueObjects
{
    public class Email
    {
        public Email(Guid id, string address, bool isPrimary, bool isExternal)
        {
            this.Id = id;
            this.Address = address;
            this.IsPrimary = isPrimary;
            this.IsExternal = isExternal;
        }

        public Guid Id { get; }

        public string Address { get; private set; }

        public bool IsPrimary { get; private set; }

        public bool IsExternal { get; private set; }
    }
}