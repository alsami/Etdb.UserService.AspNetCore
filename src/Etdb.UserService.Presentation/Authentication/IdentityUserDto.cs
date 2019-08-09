using System;

namespace Etdb.UserService.Presentation.Authentication
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class IdentityUserDto
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string[] Roles { get; set; } = null!;

        public string AuthenticationProvider { get; set; } = null!;

        public string? ProfileImageUrl { get; set; }

        public IdentityUserDto(Guid id, string? firstName, string? lastName, string userName, string email,
            string[] roles, string authenticationProvider, string? profileImageUrl)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UserName = userName;
            this.Email = email;
            this.Roles = roles;
            this.AuthenticationProvider = authenticationProvider;
            this.ProfileImageUrl = profileImageUrl;
        }

        public IdentityUserDto()
        {
        }
    }
}