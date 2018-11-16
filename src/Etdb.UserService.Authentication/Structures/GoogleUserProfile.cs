using Newtonsoft.Json;

namespace Etdb.UserService.Authentication.Structures
{
    internal class GoogleUserProfile
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public bool Verified { get; set; }

        public string Given_Name { get; set; }

        public string Family_Name { get; set; }

        public string Link { get; set; }

        public string Picture { get; set; }
    }
}