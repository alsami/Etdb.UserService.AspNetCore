using System;
using System.Net;
using Etdb.ServiceBase.Cqrs.Abstractions.Messages;

namespace Etdb.UserService.Cqrs.Abstractions.Events.Authentication
{
    public class UserAuthenticatedEvent : IEvent
    {
        public string AuthenticationLogType { get; }

        public IPAddress IpAddress { get; }

        public Guid UserId { get; }

        public DateTime LoggedAt { get; }

        public string? AdditionalInfo { get; }

        public UserAuthenticatedEvent(string authenticationLogType, IPAddress ipAddress, Guid userId, DateTime loggedAt,
            string? additionalInfo)
        {
            this.AuthenticationLogType = authenticationLogType;
            this.IpAddress = ipAddress;
            this.UserId = userId;
            this.LoggedAt = loggedAt;
            this.AdditionalInfo = additionalInfo;
        }
    }
}