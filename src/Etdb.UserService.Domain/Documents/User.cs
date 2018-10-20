using System;
using System.Collections.Generic;
using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Domain.Documents
{
    public class User : GuidDocument
    {
        public User(Guid id, string userName, string firstName, string name, string biography, string password,
            byte[] salt,
            DateTime registeredSince, UserProfileImage profileImage, ICollection<Guid> roleIds,
            ICollection<Email> emails) : base(id)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.Name = name;
            this.Biography = biography;
            this.Password = password;
            this.Salt = salt;
            this.RegisteredSince = registeredSince;
            this.ProfileImage = profileImage;
            this.RoleIds = roleIds;
            this.Emails = emails;
        }

        public string UserName { get; }

        public string FirstName { get; }

        public string Name { get; }

        public string Biography { get; }

        public string Password { get; }

        public byte[] Salt { get; }

        public DateTime RegisteredSince { get; }

        public UserProfileImage ProfileImage { get; }

        public ICollection<Guid> RoleIds { get; }

        public ICollection<Email> Emails { get; }
    }
}