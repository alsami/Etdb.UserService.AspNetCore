using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.EventSourcing.Abstractions.Handler;
using Etdb.UserService.EventSourcing.Events;

namespace Etdb.UserService.EventSourcing.EventHandler
{
    public class UserUpdateEventHandler : IDomainEventHandler<UserUpdateEvent>
    {
        public Task Handle(UserUpdateEvent notification, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
