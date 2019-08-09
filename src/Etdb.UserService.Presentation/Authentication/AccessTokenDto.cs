using System;

namespace Etdb.UserService.Presentation.Authentication
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable MemberCanBePrivate.Global
    public class AccessTokenDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string TokenType { get; set; } = null!;

        public string AuthenticationProvider { get; set; } = null!;

        public AccessTokenDto(string accessToken, string refreshToken, DateTime expiresAt, string tokenType,
            string authenticationProvider)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.ExpiresAt = expiresAt;
            this.TokenType = tokenType;
            this.AuthenticationProvider = authenticationProvider;
        }

        public AccessTokenDto()
        {
        }
    }
}