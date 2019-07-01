using System;
using System.Collections.Generic;
using System.Linq;
using Etdb.UserService.Domain.Base;
using Etdb.UserService.Domain.Enums;
using Newtonsoft.Json;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Etdb.UserService.Domain.Entities
{
    public class User : GuidDocument
    {


        [JsonConstructor]
        private User(Guid id, string userName, string firstName, string name, string biography,
            DateTime registeredSince, IEnumerable<Guid> roleIds,
            IEnumerable<Email> emails,
            AuthenticationProvider authenticationProvider = AuthenticationProvider.UsernamePassword,
            string password = null, byte[] salt = null, IEnumerable<ProfileImage> profileImages = null,
            IEnumerable<AuthenticationLog> authenticationLogs = null) : base(id)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.Name = name;
            this.Biography = biography;
            this.RegisteredSince = registeredSince;
            this.AuthenticationProvider = authenticationProvider;
            this.Password = password;
            this.Salt = salt;
            this.ProfileImages = profileImages?.ToList() ?? new List<ProfileImage>();
            this.AuthenticationLogs = authenticationLogs?.ToList() ?? new List<AuthenticationLog>();
            this.Emails = emails?.ToList() ?? new List<Email>();
            this.RoleIds = roleIds?.ToList() ?? new List<Guid>();
        }

        public string UserName { get; private set; }

        public string FirstName { get; private set; }

        public string Name { get; private set; }

        public string Biography { get; private set; }

        public DateTime RegisteredSince { get; private set; }

        public AuthenticationProvider AuthenticationProvider { get; private set; }

        public string Password { get; private set; }

        public byte[] Salt { get; private set; }

        public IReadOnlyCollection<ProfileImage> ProfileImages { get; private set; }

        public IReadOnlyCollection<AuthenticationLog> AuthenticationLogs { get; private set; }
        
        public IReadOnlyCollection<Guid> RoleIds  { get; private set; }

        public IReadOnlyCollection<Email> Emails  { get; private set; }

        public void AddAuthenticationLog(AuthenticationLog authenticationLog)
        {
            var shadowCopy = this.AuthenticationLogs.ToList();
            shadowCopy.Add(authenticationLog);
            this.AuthenticationLogs = shadowCopy;
        }

        public void RemoveAuthenticationLogs(Predicate<AuthenticationLog> predicate)
        {
            var shadowCopy = this.AuthenticationLogs.ToList();
            shadowCopy.RemoveAll(predicate);
            this.AuthenticationLogs = shadowCopy;
        }

        public void AddEmailAddress(Email email)
        {
            var copy = this.Emails.ToList();
            copy.Add(email);
            this.Emails = copy;
        }

        public void AddProfileImage(ProfileImage profileImage)         {
            var copy = this.ProfileImages.ToList();
            copy.Add(profileImage);
            this.ProfileImages = copy;
        }

        public void RemoveProfimeImage(Func<ProfileImage, bool> predicate)
        {
            var image = this.ProfileImages.FirstOrDefault(predicate);

            if (image == null) return;

            var copy = this.ProfileImages.ToList();
            copy.Remove(image);

            this.ProfileImages = copy;
        }

        public static User Create(Guid id, string userName, string firstName, string name, string biography,
            DateTime registeredSince, IEnumerable<Guid> roleIds,
            IEnumerable<Email> emails,
            AuthenticationProvider authenticationProvider = AuthenticationProvider.UsernamePassword,
            string password = null, byte[] salt = null, IEnumerable<ProfileImage> profileImages = null,
            IEnumerable<AuthenticationLog> authenticationLogs = null) => new User(id, userName, firstName, name,
            biography, registeredSince, roleIds, emails, authenticationProvider, password, salt, profileImages,
            authenticationLogs);

        public User MutateUserName(string userName) => Create(this.Id, userName, this.FirstName, this.Name,
            this.Biography, this.RegisteredSince, this.RoleIds, this.Emails, this.AuthenticationProvider, this.Password,
            this.Salt, this.ProfileImages, this.AuthenticationLogs);

        public User MutateProfileInfo(string firstName, string name, string biography)
            => Create(this.Id, this.UserName, firstName, name, biography, this.RegisteredSince, this.RoleIds,
                this.Emails, this.AuthenticationProvider, this.Password,
                this.Salt, this.ProfileImages, this.AuthenticationLogs);

        public User MutateCredentials(string password, byte[] salte)
            => Create(this.Id, this.UserName, this.FirstName, this.Name, this.Biography, this.RegisteredSince,
                this.RoleIds, this.Emails, this.AuthenticationProvider, password, salte, this.ProfileImages,
                this.AuthenticationLogs);
    }
}