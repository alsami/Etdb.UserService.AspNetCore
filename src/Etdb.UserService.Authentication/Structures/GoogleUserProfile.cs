// ReSharper disable InconsistentNaming
namespace Etdb.UserService.Authentication.Structures
{
    internal class GoogleUserProfile
    {
        public string Email { get; }

        public string Given_Name { get; }

        public string Family_Name { get; }

        public string Picture { get; }

        public GoogleUserProfile(string email, string givenName, string familyName, string picture)
        {
            this.Email = email;
            this.Given_Name = givenName;
            this.Family_Name = familyName;
            this.Picture = picture;
        }
    }
}