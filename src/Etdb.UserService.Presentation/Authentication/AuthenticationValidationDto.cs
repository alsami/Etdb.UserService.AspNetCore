using System;
using Etdb.UserService.Presentation.Enums;

namespace Etdb.UserService.Presentation.Authentication
{
    public class AuthenticationValidationDto
    {
        public bool IsValid { get; }

        public AuthenticationFailure? AuthenticationFailure { get; }

        public Guid UserId { get; }

        public AuthenticationValidationDto(bool isValid, AuthenticationFailure? authenticationFailure = null,
            Guid? userId = null)
        {
            this.AuthenticationFailure = authenticationFailure;
            this.IsValid = isValid;
            this.UserId = userId.GetValueOrDefault();
        }
    }
}