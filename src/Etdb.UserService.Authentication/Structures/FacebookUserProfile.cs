namespace Etdb.UserService.Authentication.Structures
{
    internal class FacebookUserProfile
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public FacebookUserProfilePicture Picture { get; set; }
    }
}