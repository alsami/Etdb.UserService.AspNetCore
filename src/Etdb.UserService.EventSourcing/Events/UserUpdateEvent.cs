using Etdb.UserService.EventSourcing.Abstractions.Events;

namespace Etdb.UserService.EventSourcing.Events
{
    public class UserUpdateEvent : UserEvent
    {
        public UserUpdateEvent(string id, string name, string lastName, string email, string userName) : base(id, name, lastName, email, userName)
        {
        }
    }
}
