namespace Etdb.UserService.Misc.Configuration
{
    public class Client
    {
        public string Id { get; set; } = null!;

        public string Secret { get; set; } = null!;

        public bool HasOfflineAccess { get; set; }

        public string[] Scopes { get; set; } = null!;

        public string[] GrantTypes { get; set; } = null!;

        public string[] Origins { get; set; } = null!;
    }
}