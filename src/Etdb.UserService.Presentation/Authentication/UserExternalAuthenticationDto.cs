namespace Etdb.UserService.Presentation.Authentication
{
    public class UserExternalAuthenticationDto
    {
        public string ClientId { get; }

        public string Token { get; }

        public string Provider { get; }

        public UserExternalAuthenticationDto(string clientId, string token, string provider)
        {
            this.ClientId = clientId;
            this.Token = token;
            this.Provider = provider;
        }
    }
}