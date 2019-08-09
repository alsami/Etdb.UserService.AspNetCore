using System.Collections.Generic;

namespace Etdb.UserService.Presentation.Users
{
    public class UserRegisterDto
    {
        public string FirstName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public ICollection<AddEmailDto> Emails { get; set; } = null!;

        public UserRegisterDto(string firstName, string name, string userName, string password,
            ICollection<AddEmailDto> emails)
        {
            this.FirstName = firstName;
            this.Name = name;
            this.UserName = userName;
            this.Password = password;
            this.Emails = emails;
        }

        public UserRegisterDto()
        {
        }
    }
}