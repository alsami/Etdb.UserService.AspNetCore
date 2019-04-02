namespace Etdb.UserService.Presentation.Authentication
{
    public class UserInternalAuthenticationDto
    {
        public string Username { get; }
        public string Password { get; }
        public string ClientId { get; }

        public UserInternalAuthenticationDto(string username, string password, string clientId)
        {
            this.Username = username;
            this.Password = password;
            this.ClientId = clientId;
        }
    }
}