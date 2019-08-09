using System;

namespace Etdb.UserService.Presentation.Authentication
{
    public class AuthenticationLogDto
    {
        public DateTime LoggedAt { get; set; }

        public string AuthenticationLogType { get; set; } = null!;

        public string? IpAddress { get; set; } = null!;

        public string? AdditionalInformation { get; set; } = null!;

        public AuthenticationLogDto(DateTime loggedAt, string authenticationLogType, string? ipAddress,
            string? additionalInformation)
        {
            this.LoggedAt = loggedAt;
            this.AuthenticationLogType = authenticationLogType;
            this.IpAddress = ipAddress;
            this.AdditionalInformation = additionalInformation;
        }

        public AuthenticationLogDto()
        {
        }
    }
}