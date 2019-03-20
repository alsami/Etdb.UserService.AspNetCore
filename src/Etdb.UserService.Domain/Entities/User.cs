using System;
using System.Collections.Generic;
using Etdb.UserService.Domain.Base;
using Etdb.UserService.Domain.Enums;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Etdb.UserService.Domain.Entities
{
    public class User : GuidDocument
    {
        public User(Guid id, string userName, string firstName, string name, string biography,
            DateTime registeredSince, ICollection<Guid> roleIds,
            ICollection<Email> emails,
            AuthenticationProvider authenticationProvider = AuthenticationProvider.UsernamePassword,
            string password = null, byte[] salt = null, UserProfileImage profileImage = null) : base(id)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.Name = name;
            this.Biography = biography;
            this.RegisteredSince = registeredSince;
            this.RoleIds = roleIds;
            this.Emails = emails;
            this.AuthenticationProvider = authenticationProvider;
            this.Password = password;
            this.Salt = salt;
            this.ProfileImage = profileImage;
        }

        public string UserName { get; private set; }

        public string FirstName { get; private set; }

        public string Name { get; private set; }

        public string Biography { get; private set; }

        public DateTime RegisteredSince { get; private set; }

        public UserProfileImage ProfileImage { get; private set; }

        public ICollection<Guid> RoleIds { get; private set; }

        public ICollection<Email> Emails { get; private set; }

        public AuthenticationProvider AuthenticationProvider { get; private set; }

        public string Password { get; private set; }

        public byte[] Salt { get; private set; }
    }
}