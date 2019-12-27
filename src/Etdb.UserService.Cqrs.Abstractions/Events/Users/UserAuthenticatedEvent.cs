using System;
using System.Net;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Events.Authentication
{
    public class UserAuthenticatedEvent : INotification
    {
        public UserAuthenticatedEvent(string authenticationLogType, IPAddress ipAddress, Guid userId, DateTime loggedAt,
            string? additionalInfo)
        {
            this.AuthenticationLogType = authenticationLogType;
            this.IpAddress = ipAddress?.ToString() ?? "127.0.0.1";
            this.UserId = userId;
            this.LoggedAt = loggedAt;
            this.AdditionalInfo = additionalInfo;
        }

        public string AuthenticationLogType { get; }

        public string IpAddress { get; }

        public Guid UserId { get; }

        public DateTime LoggedAt { get; }

        public string? AdditionalInfo { get; }
    }
}