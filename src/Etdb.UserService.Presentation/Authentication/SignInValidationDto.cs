using System;
using Etdb.UserService.Presentation.Enums;

namespace Etdb.UserService.Presentation.Authentication
{
    public class SignInValidationDto
    {
        public bool IsValid { get; }

        public SignInFailure? signInFailure { get; }

        public Guid UserId { get; }

        public SignInValidationDto(bool isValid, SignInFailure? signInFailure = null, Guid? userId = null)
        {
            this.signInFailure = signInFailure;
            this.IsValid = isValid;
            this.UserId = userId.GetValueOrDefault();
        }
    }
}