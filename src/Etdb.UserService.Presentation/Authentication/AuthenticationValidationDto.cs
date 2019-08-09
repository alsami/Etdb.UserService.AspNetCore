using System;
using Etdb.UserService.Presentation.Enums;

namespace Etdb.UserService.Presentation.Authentication
{
    public class AuthenticationValidationDto
    {
        public bool IsValid { get; set; }

        public AuthenticationFailure? AuthenticationFailure { get; set; }

        public Guid UserId { get; set; }

        public AuthenticationValidationDto(bool isValid, AuthenticationFailure? authenticationFailure = null,
            Guid? userId = null)
        {
            this.AuthenticationFailure = authenticationFailure;
            this.IsValid = isValid;
            this.UserId = userId.GetValueOrDefault();
        }

        public AuthenticationValidationDto()
        {
        }
    }
}