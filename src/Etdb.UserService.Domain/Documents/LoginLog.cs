using System;
using Etdb.UserService.Domain.Base;
using Etdb.UserService.Domain.Enums;

namespace Etdb.UserService.Domain.Documents
{
    public class LoginLog : GuidDocument
    {
        public LoginLog(Guid id, Guid userId, DateTime loggedAt, LoginType loginType, string ipAddress,
            string additionalInformation) : base(id)
        {
            UserId = userId;
            LoggedAt = loggedAt;
            LoginType = loginType;
            IpAddress = ipAddress;
            AdditionalInformation = additionalInformation;
        }

        public Guid UserId { get; private set; }

        public DateTime LoggedAt { get; private set; }

        public LoginType LoginType { get; private set; }

        public string IpAddress { get; private set; }

        public string AdditionalInformation { get; private set; }
    }
}