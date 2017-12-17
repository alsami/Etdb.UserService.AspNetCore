using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.EventSourcing.Handler;
using Etdb.UserService.EventSourcing.Events;

namespace Etdb.UserService.EventSourcing.EventHandler
{
    public class UserRegisterEventHandler : DomainEventHandler<UserRegisterEvent>
    {
        public override Task Handle(UserRegisterEvent notification, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
