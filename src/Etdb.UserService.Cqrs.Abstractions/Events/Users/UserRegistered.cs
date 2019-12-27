using System;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Events.Users
{
    public class UserRegisteredEvent : INotification
    {
        public UserRegisteredEvent(Guid userId, string userName, DateTime registeredAt)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.RegisteredAt = registeredAt;
        }

        public Guid UserId { get; }

        public string UserName { get; }

        public DateTime RegisteredAt { get; }
    }
}