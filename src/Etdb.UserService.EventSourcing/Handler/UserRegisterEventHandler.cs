using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.EventSourcing.Handler;
using Etdb.UserService.EventSourcing.Events;

namespace Etdb.UserService.EventSourcing.Handler
{
    public class UserRegisterEventHandler : DomainEventHandler<UserRegisterEvent>
    {
        public override void Handle(UserRegisterEvent notification)
        {
        }

        public override Task Handle(UserRegisterEvent notification, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
