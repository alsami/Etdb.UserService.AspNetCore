namespace Etdb.UserService.Presentation.Authentication
{
    public class InternalAuthenticationDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ClientId { get; set; } = null!;

        public InternalAuthenticationDto(string username, string password, string clientId)
        {
            this.Username = username;
            this.Password = password;
            this.ClientId = clientId;
        }

        public InternalAuthenticationDto()
        {
        }
    }
}