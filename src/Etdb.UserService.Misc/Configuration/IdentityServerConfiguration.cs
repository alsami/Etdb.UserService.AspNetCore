namespace Etdb.UserService.Misc.Configuration
{
    public class IdentityServerConfiguration
    {
        public Client[] Clients { get; set; } = null!;

        public string Authority { get; set; } = null!;
    }
}