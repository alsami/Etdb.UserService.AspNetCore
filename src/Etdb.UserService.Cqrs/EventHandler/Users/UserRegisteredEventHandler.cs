using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Events.Users;
using Etdb.UserService.Services.Abstractions;
using MediatR;

namespace Etdb.UserService.Cqrs.EventHandler.Users
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IMessageProducerAdapter messageProducerAdapter;

        public UserRegisteredEventHandler(IMessageProducerAdapter messageProducerAdapter)
        {
            this.messageProducerAdapter = messageProducerAdapter;
        }

        public Task Handle(UserRegisteredEvent message, CancellationToken cancellationToken)
            => this.messageProducerAdapter.ProduceAsync(message, MessageType.UserRegistered);
    }
}