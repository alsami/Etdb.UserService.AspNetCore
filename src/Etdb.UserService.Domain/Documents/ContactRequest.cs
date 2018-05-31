using System;
using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Domain.Documents
{
    public class ContactRequest : GuidDocument
    {
        public ContactRequest(Guid id, Guid senderId, Guid receiverId, DateTime send) : base(id)
        {
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
            this.Send = send;
        }

        public Guid SenderId { get; }

        public Guid ReceiverId { get; }

        public DateTime Send { get; }

        public bool Accepted { get; }

        public DateTime? AcceptedAt { get; }

        public bool Rejected { get; }

        public bool? RejectedAt { get; }
    }
}