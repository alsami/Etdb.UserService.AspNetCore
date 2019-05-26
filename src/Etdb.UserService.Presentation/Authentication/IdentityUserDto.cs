using System;

namespace Etdb.UserService.Presentation.Authentication
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class IdentityUserDto
    {
        public Guid Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string UserName { get; }

        public string[] Emails { get; }

        public string[] Roles { get; }

        public string AuthenticationProvider { get; }

        public string ProfileImageUrl { get; }

        public IdentityUserDto(Guid id, string firstName, string lastName, string userName, string[] emails,
            string[] roles, string authenticationProvider, string profileImageUrl)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UserName = userName;
            this.Emails = emails;
            this.Roles = roles;
            this.AuthenticationProvider = authenticationProvider;
            this.ProfileImageUrl = profileImageUrl;
        }
    }
}