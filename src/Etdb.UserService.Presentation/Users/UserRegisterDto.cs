using System.Collections.Generic;

namespace Etdb.UserService.Presentation.Users
{
    public class UserRegisterDto
    {
        public string FirstName { get; }

        public string Name { get; }

        public string UserName { get; }

        public string Password { get; }

        public ICollection<AddEmailDto> Emails { get; }

        public UserRegisterDto(string firstName, string name, string userName, string password, ICollection<AddEmailDto> emails)
        {
            this.FirstName = firstName;
            this.Name = name;
            this.UserName = userName;
            this.Password = password;
            this.Emails = emails;
        }
    }
}