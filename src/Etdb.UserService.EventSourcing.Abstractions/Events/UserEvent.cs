using System;
using Etdb.ServiceBase.EventSourcing.Abstractions.Events;

namespace Etdb.UserService.EventSourcing.Abstractions.Events
{
    public abstract class UserEvent : Event
    {
        protected UserEvent(Guid id, string name, string lastName, string email, string userName)
        {
            this.Id = id;
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.UserName = userName;
            this.AggregateId = id;
        }

        public Guid Id
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string LastName
        {
            get;
            protected set;
        }

        public string UserName
        {
            get;
            protected set;
        }

        public string Email
        {
            get;
            protected set;
        }
    }
}
