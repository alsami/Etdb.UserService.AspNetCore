using System;
using System.Net;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Events.Users
{
    public class UserAuthenticatedEvent : INotification
    {
        public UserAuthenticatedEvent(Guid userId, string userName, string authenticationLogType, IPAddress ipAddress,  DateTime loggedAt,
            string? additionalInfo)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.AuthenticationLogType = authenticationLogType;
            this.IpAddress = ipAddress?.ToString() ?? "127.0.0.1";
            this.LoggedAt = loggedAt;
            this.AdditionalInfo = additionalInfo;
        }
        
        public Guid UserId { get; }

        public string UserName { get; }

        public string AuthenticationLogType { get; }

        public string IpAddress { get; }

        public DateTime LoggedAt { get; }

        public string? AdditionalInfo { get; }
    }
}