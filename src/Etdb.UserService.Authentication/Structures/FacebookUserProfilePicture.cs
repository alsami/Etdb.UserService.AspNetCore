namespace Etdb.UserService.Authentication.Structures
{
    internal class FacebookUserProfilePicture
    {
        public FacebookUserProfilePictureData Data { get; }

        public FacebookUserProfilePicture(FacebookUserProfilePictureData data)
        {
            this.Data = data;
        }
    }
}