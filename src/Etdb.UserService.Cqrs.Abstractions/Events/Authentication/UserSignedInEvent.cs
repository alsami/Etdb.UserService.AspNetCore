using System;
using System.Net;
using Etdb.ServiceBase.Cqrs.Abstractions.Messages;

namespace Etdb.UserService.Cqrs.Abstractions.Events.Authentication
{
    public class UserSignedInEvent : IEvent
    {
        public string SignInEventType { get; }

        public IPAddress IpAddress { get; }

        public Guid UserId { get; }

        public DateTime LoggedAt { get; }

        public string AdditionalInfo { get; }

        public UserSignedInEvent(string signInEventType, IPAddress ipAddress, Guid userId, DateTime loggedAt,
            string additionalInfo)
        {
            this.SignInEventType = signInEventType;
            this.IpAddress = ipAddress;
            this.UserId = userId;
            this.LoggedAt = loggedAt;
            this.AdditionalInfo = additionalInfo;
        }
    }
}