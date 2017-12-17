using System;
using System.Collections.Generic;
using System.Text;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Abstractions.Events;

namespace Etdb.UserService.EventSourcing.Events
{
    public class UserUpdateEvent : UserEvent
    {
        public UserUpdateEvent(Guid id, string name, string lastName, string email, string userName, string password, byte[] salt, byte[] rowVersion, ICollection<UserSecurityrole> userSecurityroles) : base(id, name, lastName, email, userName, password, salt, rowVersion, userSecurityroles)
        {
        }
    }
}
