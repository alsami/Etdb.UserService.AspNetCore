namespace Etdb.UserService.Presentation.Authentication
{
    public class ExternalAuthenticationDto
    {
        public string ClientId { get; }

        public string Token { get; }

        public string Provider { get; }

        public ExternalAuthenticationDto(string clientId, string token, string provider)
        {
            this.ClientId = clientId;
            this.Token = token;
            this.Provider = provider;
        }
    }
}