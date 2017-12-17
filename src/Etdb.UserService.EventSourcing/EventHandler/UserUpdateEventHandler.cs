using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.EventSourcing.Handler;
using Etdb.UserService.EventSourcing.Events;

namespace Etdb.UserService.EventSourcing.EventHandler
{
    public class UserUpdateEventHandler : DomainEventHandler<UserUpdateEvent>
    {
        public override Task Handle(UserUpdateEvent notification, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
