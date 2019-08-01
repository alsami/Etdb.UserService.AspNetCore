namespace Etdb.UserService.Authentication.Structures
{
    internal class FacebookUserProfile
    {
        public string Email { get; }

        public string Name { get; }

        public FacebookUserProfilePicture Picture { get; }

        public FacebookUserProfile(string email, string name, FacebookUserProfilePicture picture)
        {
            this.Email = email;
            this.Name = name;
            this.Picture = picture;
        }
    }
}