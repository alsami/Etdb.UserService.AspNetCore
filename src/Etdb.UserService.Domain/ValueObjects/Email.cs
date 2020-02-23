using System;
using Newtonsoft.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Etdb.UserService.Domain.ValueObjects
{
    public class Email
    {
        [JsonConstructor]
        private Email(Guid id, string address, bool isPrimary, bool isExternal)
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
        
        public static Email Create(Guid id, string address, bool isPrimary, bool isExternal = false)
            => new Email(id, address, isPrimary, isExternal);
    }
}