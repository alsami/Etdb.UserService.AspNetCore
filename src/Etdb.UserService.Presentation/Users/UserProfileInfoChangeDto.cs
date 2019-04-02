namespace Etdb.UserService.Presentation.Users
{
    public class UserProfileInfoChangeDto
    {
        public string FirstName { get; }

        public string Name { get; }

        public string Biography { get; }

        public UserProfileInfoChangeDto(string firstName, string name, string biography)
        {
            this.FirstName = firstName;
            this.Name = name;
            this.Biography = biography;
        }
    }
}