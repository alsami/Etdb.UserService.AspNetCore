namespace Etdb.UserService.Presentation.Authentication
{
    public class ExternalAuthenticationDto
    {
        public string ClientId { get; set; } = null!;

        public string Token { get; set; } = null!;

        public string Provider { get; set; } = null!;

        public ExternalAuthenticationDto(string clientId, string token, string provider)
        {
            this.ClientId = clientId;
            this.Token = token;
            this.Provider = provider;
        }

        public ExternalAuthenticationDto()
        {
        }
    }
}