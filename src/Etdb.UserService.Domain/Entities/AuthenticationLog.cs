using System;
using Etdb.UserService.Domain.Base;
using Etdb.UserService.Domain.Enums;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.Entities
{
    public class AuthenticationLog : UserChildDocument
    {
        public AuthenticationLog(Guid id, Guid userId, DateTime loggedAt, AuthenticationLogType authenticationLogType, string ipAddress,
            string additionalInformation) : base(id, userId)
        {
            this.UserId = userId;
            this.LoggedAt = loggedAt;
            this.AuthenticationLogType = authenticationLogType;
            this.IpAddress = ipAddress;
            this.AdditionalInformation = additionalInformation;
        }

        public DateTime LoggedAt { get; private set; }

        public AuthenticationLogType AuthenticationLogType { get; private set; }

        public string IpAddress { get; private set; }

        public string AdditionalInformation { get; private set; }
    }
}