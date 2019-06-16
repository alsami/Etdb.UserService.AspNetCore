namespace Etdb.UserService.Misc.Configuration
{
    public class Client
    {
        public string Id { get; set; }

        public string Secret { get; set; }

        public bool HasOfflineAccess { get; set; }

        public string[] Scopes { get; set; }

        public string[] GrantTypes { get; set; }

        public string[] Origins { get; set; }
    }
}