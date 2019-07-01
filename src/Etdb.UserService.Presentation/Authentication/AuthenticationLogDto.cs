using System;

namespace Etdb.UserService.Presentation.Authentication
{
    public class AuthenticationLogDto
    {
        public DateTime LoggedAt { get; private set; }

        public string AuthenticationLogType { get; private set; }

        public string IpAddress { get; private set; }

        public string AdditionalInformation { get; private set; }

        public AuthenticationLogDto(DateTime loggedAt, string authenticationLogType, string ipAddress,
            string additionalInformation)
        {
            this.LoggedAt = loggedAt;
            this.AuthenticationLogType = authenticationLogType;
            this.IpAddress = ipAddress;
            this.AdditionalInformation = additionalInformation;
        }
    }
}