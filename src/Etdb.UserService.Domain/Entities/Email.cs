using System;
using Etdb.UserService.Domain.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Etdb.UserService.Domain.Entities
{
    public class Email : UserChildDocument
    {
        public Email(Guid id, Guid userId, string address, bool isPrimary, bool isExternal) : base(id, userId)
        {
            this.UserId = userId;
            this.Address = address;
            this.IsPrimary = isPrimary;
            this.IsExternal = isExternal;
        }

        public string Address { get; private set; }

        public bool IsPrimary { get; private set; }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool IsExternal { get; private set; }
    }
}