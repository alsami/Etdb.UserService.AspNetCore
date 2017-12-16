using Etdb.UserService.EventSourcing.Events;
using ETDB.API.ServiceBase.EventSourcing.Handler;

namespace Etdb.UserService.EventSourcing.Handler
{
    public class UserRegisterEventHandler : DomainEventHandler<UserRegisterEvent>
    {
        public override void Handle(UserRegisterEvent notification)
        {
            
        }
    }
}
