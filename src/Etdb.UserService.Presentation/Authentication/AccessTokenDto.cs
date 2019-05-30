using System;

namespace Etdb.UserService.Presentation.Authentication
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable MemberCanBePrivate.Global
    public class AccessTokenDto
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime ExpiresAt { get; }
        public string TokenType { get; }

        public string AuthenticationProvider { get; }

        public AccessTokenDto(string accessToken, string refreshToken, DateTime expiresAt, string tokenType, string authenticationProvider)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.ExpiresAt = expiresAt;
            this.TokenType = tokenType;
            this.AuthenticationProvider = authenticationProvider;
        }
    }
}