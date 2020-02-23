using System;
using Etdb.UserService.Domain.Enums;
using Newtonsoft.Json;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.ValueObjects
{
    public class AuthenticationLog
    {
        [JsonConstructor]
        private AuthenticationLog(Guid id, DateTime loggedAt, AuthenticationLogType authenticationLogType,
            string? ipAddress = null,
            string? additionalInformation = null)
        {
            this.Id = id;
            this.LoggedAt = loggedAt;
            this.AuthenticationLogType = authenticationLogType;
            this.IpAddress = ipAddress;
            this.AdditionalInformation = additionalInformation;
        }

        public Guid Id { get; private set; }

        public DateTime LoggedAt { get; private set; }

        public AuthenticationLogType AuthenticationLogType { get; private set; }

        public string? IpAddress { get; private set; }

        public string? AdditionalInformation { get; private set; }
        
        public static AuthenticationLog Create(Guid id, DateTime loggedAt, AuthenticationLogType authenticationLogType, string? ipAddress = null, string? additionalInformation = null)
            => new AuthenticationLog(id, loggedAt, authenticationLogType, ipAddress, additionalInformation);
    }
}