namespace Etdb.UserService.Authentication.Structures
{
    internal class FacebookUserProfilePictureData
    {
        public string Url { get; }

        public FacebookUserProfilePictureData(string url)
        {
            this.Url = url;
        }
    }
}