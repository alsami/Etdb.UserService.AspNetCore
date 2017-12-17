using System;
using System.Collections.Generic;
using Etdb.ServiceBase.EventSourcing.Abstractions.Events;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Abstractions.Events;

namespace Etdb.UserService.EventSourcing.Events
{
    public class UserRegisterEvent : UserEvent
    {
        public UserRegisterEvent(Guid id, string name, string lastName, string email, string userName, string password, byte[] salt, byte[] rowVersion, ICollection<UserSecurityrole> userSecurityroles) : base(id, name, lastName, email, userName, password, salt, rowVersion, userSecurityroles)
        {
        }
    }
}
