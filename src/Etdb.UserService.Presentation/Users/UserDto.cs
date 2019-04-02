using System;
using System.Collections.Generic;
using Etdb.UserService.Presentation.Base;

// ReSharper disable UnusedMember.Global

namespace Etdb.UserService.Presentation.Users
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class UserDto : GuidDto
    {
        public string UserName { get; }

        public string FirstName { get; }

        public string Name { get; }

        public string Biography { get; }

        public DateTime RegisteredSince { get; }

        public string ProfileImageUrl { get; }

        public string SignInProvider { get; }

        public bool IsExternalUser { get; }

        public ICollection<EmailDto> Emails { get; }

        public UserDto(Guid id, string userName, string firstName, string name, string biography, DateTime registeredSince, string profileImageUrl, string signInProvider, bool isExternalUser, ICollection<EmailDto> emails) : base(id)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.Name = name;
            this.Biography = biography;
            this.RegisteredSince = registeredSince;
            this.ProfileImageUrl = profileImageUrl;
            this.SignInProvider = signInProvider;
            this.IsExternalUser = isExternalUser;
            this.Emails = emails;
        }
    }
}