using System;
using Etdb.ServiceBase.Domain.Abstractions.Documents;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Etdb.UserService.Domain.ValueObjects
{
    public class Email : IDocument<Guid>
    {
        public Email(Guid id, string address, bool isPrimary, bool isExternal)
        {
            this.Id = id;
            this.Address = address;
            this.IsPrimary = isPrimary;
            this.IsExternal = isExternal;
        }

        public Guid Id { get; private set; }
        
        public string Address { get; private set; }

        public bool IsPrimary { get; private set; }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool IsExternal { get; private set; }
    }
}