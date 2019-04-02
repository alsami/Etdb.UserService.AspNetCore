using System;
using Etdb.UserService.Domain.Base;
using Etdb.UserService.Domain.Enums;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.Entities
{
    public class SignInLog : GuidDocument
    {
        public SignInLog(Guid id, Guid userId, DateTime loggedAt, SignInType signInType, string ipAddress,
            string additionalInformation) : base(id)
        {
            this.UserId = userId;
            this.LoggedAt = loggedAt;
            this.SignInType = signInType;
            this.IpAddress = ipAddress;
            this.AdditionalInformation = additionalInformation;
        }

        public Guid UserId { get; private set; }

        public DateTime LoggedAt { get; private set; }

        public SignInType SignInType { get; private set; }

        public string IpAddress { get; private set; }

        public string AdditionalInformation { get; private set; }
    }
}