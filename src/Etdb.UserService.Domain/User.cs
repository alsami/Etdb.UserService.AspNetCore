using System.Collections.Generic;
using Etdb.UserService.Domain.Base;
using MongoDB.Driver;

namespace Etdb.UserService.Domain
{
    public class User : GuidDocument
    {
        public User()
        {
            this.Emails = new List<Email>();
        }
        
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] Salt { get; set; }
        
        public string Password { get; set; }
        
        public ICollection<Email> Emails { get; set; }
        
        public ICollection<SecurityRole> SecurityRoles { get; set; }
        
        public ICollection<MongoDBRef> SecurityRoleReferences { get; set; }
    }
}