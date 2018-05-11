using System;
using System.Collections.Generic;
using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Domain
{
    public class User : GuidDocument
    {
        public User(Guid id, string userName, string firstName, string name, byte[] salt, string password, ICollection<Guid> roleIds, 
            ICollection<Email> emails) : base(id)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.Name = name;
            this.Salt = salt;
            this.Password = password;
            this.RoleIds = roleIds;
            this.Emails = emails;
        }
        
        public string UserName { get; private set; }

        public string FirstName { get; private set; }

        public string Name { get; private set; }

        public byte[] Salt { get; private set; }
        
        public string Password { get; private set; }
        
        public ICollection<Guid> RoleIds { get; private set; }
        
        public ICollection<Email> Emails { get; private set; }
    }
}