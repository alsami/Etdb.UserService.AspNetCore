using System;
using Etdb.UserService.Presentation.Base;

namespace Etdb.UserService.Presentation
{
    public class ContactRequestDto : GuidDto
    {
        public Guid SenderId { get; }

        public Guid ReceiverId { get; }
    }
}