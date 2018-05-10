using System;
using System.Collections.Generic;
using Etdb.UserService.Domain.Base;
using Etdb.UserService.Extensions.Converters;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Etdb.UserService.Domain
{
    public class User : GuidDocument
    {
        public User(Guid id, string userName, string firstName, string name, byte[] salt, string password,
            ICollection<Email> emails, ICollection<MongoDBRef> securityRoleReferences) : base(id)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.Name = name;
            this.Salt = salt;
            this.Password = password;
            this.Emails = emails;
            this.SecurityRoleReferences = securityRoleReferences;
        }
        
        public string UserName { get; private set; }

        public string FirstName { get; private set; }

        public string Name { get; private set; }

        public byte[] Salt { get; private set; }
        
        public string Password { get; private set; }
        
        public ICollection<Email> Emails { get; private set; }
        
        [JsonConverter(typeof(MongoDbRefConverter))]
        public ICollection<MongoDBRef> SecurityRoleReferences { get; private set; }
    }
}