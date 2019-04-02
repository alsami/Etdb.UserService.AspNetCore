using System;
using Etdb.UserService.Presentation.Base;

namespace Etdb.UserService.Presentation.Users
{
    public class EmailDto : GuidDto
    {
        public string Address { get; }

        public bool IsPrimary { get; }

        public EmailDto(Guid id, string address, bool isPrimary) : base(id)
        {
            this.Address = address;
            this.IsPrimary = isPrimary;
        }
    }
}