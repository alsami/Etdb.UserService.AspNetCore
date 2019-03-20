namespace Etdb.UserService.Presentation
{
    public class UserExternalAuthenticationDto
    {
        public string ClientId { get; set; }

        public string Token { get; set; }

        public string Provider { get; set; }
    }
}