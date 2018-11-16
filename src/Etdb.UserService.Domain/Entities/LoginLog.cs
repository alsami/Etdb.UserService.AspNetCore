using System;
using Etdb.UserService.Domain.Base;
using Etdb.UserService.Domain.Enums;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.Entities
{
    public class LoginLog : GuidDocument
    {
        public LoginLog(Guid id, Guid userId, DateTime loggedAt, LoginType loginType, string ipAddress,
            string additionalInformation) : base(id)
        {
            this.UserId = userId;
            this.LoggedAt = loggedAt;
            this.LoginType = loginType;
            this.IpAddress = ipAddress;
            this.AdditionalInformation = additionalInformation;
        }

        public Guid UserId { get; private set; }

        public DateTime LoggedAt { get; private set; }

        public LoginType LoginType { get; private set; }

        public string IpAddress { get; private set; }

        public string AdditionalInformation { get; private set; }
    }
}