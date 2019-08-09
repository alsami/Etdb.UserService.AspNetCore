namespace Etdb.UserService.Presentation.Users
{
    public class UserProfileInfoChangeDto
    {
        public string FirstName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Biography { get; set; } = null!;

        public UserProfileInfoChangeDto(string firstName, string name, string biography)
        {
            this.FirstName = firstName;
            this.Name = name;
            this.Biography = biography;
        }

        public UserProfileInfoChangeDto()
        {
        }
    }
}