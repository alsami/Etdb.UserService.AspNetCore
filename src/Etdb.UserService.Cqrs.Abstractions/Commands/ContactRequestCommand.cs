using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class ContactRequestCommand : IResponseCommand<ContactRequestDto>
    {
        public Guid Sender { get; }

        public Guid Receiver  { get; }

        public ContactRequestCommand(Guid sender, Guid receiver)
        {
            this.Sender = sender;
            this.Receiver = receiver;
        }
    }
}