using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.EventSourcing.Abstractions.Handler;
using Etdb.UserService.EventSourcing.Events;

namespace Etdb.UserService.EventSourcing.EventHandler
{
    public class UserRegisterEventHandler : IDomainEventHandler<UserRegisterEvent>
    {
        public Task Handle(UserRegisterEvent notification, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
