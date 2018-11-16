using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Etdb.UserService.Presentation
{
    public class UserLoginValidationDto
    {
        public bool IsValid { get; }

        public Guid UserId { get; }

        public UserLoginValidationDto(bool isValid, Guid? userId = null)
        {
            this.IsValid = isValid;
            this.UserId = userId.GetValueOrDefault();
        }
    }
}