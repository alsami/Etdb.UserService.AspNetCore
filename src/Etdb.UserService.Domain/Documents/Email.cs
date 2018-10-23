using System;
using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Domain.Documents
{
    public class Email : GuidDocument
    {
        public Email(Guid id, string address, bool isPrimary) : base(id)
        {
            this.Address = address;
            this.IsPrimary = isPrimary;
        }

        public string Address { get; private set; }

        public bool IsPrimary { get; private set; }
    }
}