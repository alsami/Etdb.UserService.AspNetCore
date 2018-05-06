using System.Collections.Generic;
using Etdb.UserService.Domain.Base;
using Etdb.UserService.Extensions.Converters;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Etdb.UserService.Domain
{
    public class User : GuidDocument
    {
        public User()
        {
            this.Emails = new List<Email>();
            this.SecurityRoleReferences = new List<MongoDBRef>();
        }
        
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] Salt { get; set; }
        
        public string Password { get; set; }
        
        public ICollection<Email> Emails { get; set; }
        
        public ICollection<SecurityRole> SecurityRoles { get; set; }
        
        [JsonConverter(typeof(MongoDbRefConverter))]
        public ICollection<MongoDBRef> SecurityRoleReferences { get; set; }
    }
}