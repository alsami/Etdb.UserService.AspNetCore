using ETDB.API.ServiceBase.Abstractions.Handler;
using ETDB.API.UserService.EventSourcing.Events;

namespace ETDB.API.UserService.EventSourcing.Handler
{
    public class UserRegisterEventHandler : DomainEventHandler<UserRegisterEvent>
    {
        public override void Handle(UserRegisterEvent notification)
        {
            
        }
    }
}
