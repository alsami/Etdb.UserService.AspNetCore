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

        public string Address { get; }

        public bool IsPrimary { get; }

        public Email Clone()
        {
            return new Email(this.Id, this.Address, this.IsPrimary);
        }
    }
}