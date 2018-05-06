using System;
using System.Collections.Generic;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.UserService.Constants;
using Etdb.UserService.Domain;
using MongoDB.Driver;

namespace Etdb.UserService.Repositories.Context
{
    internal class ContextScaffold
    {        
        public static void Scaffold(UserServiceDbContext context)
        {
            var securityRoleCollection =
                context.Database.GetCollection<SecurityRole>($"{nameof(SecurityRole).ToLower()}s");

            var memberRole = securityRoleCollection.Find(role => role.Name == RoleNames.Member).FirstOrDefault();

            if (memberRole == null)
            {
                memberRole = new SecurityRole
                {
                    Id = Guid.NewGuid(),
                    Name = RoleNames.Member
                };
                
                securityRoleCollection.InsertOne(memberRole);
            }
            
            var adminRole = securityRoleCollection.Find(role => role.Name == RoleNames.Admin).FirstOrDefault();
            
            if (adminRole == null)
            {
                adminRole = new SecurityRole
                {
                    Id = Guid.NewGuid(),
                    Name = RoleNames.Admin
                };
                
                securityRoleCollection.InsertOne(adminRole);
            }

            var usersCollection = context.Database.GetCollection<User>($"{nameof(User).ToLower()}s");
            
            var adminUser =
                usersCollection.Find(user => user.UserName == "admin")
                    .FirstOrDefault();
            
            if (adminUser != null)
            {
                return;
            }
            
            var hasher = new Hasher();

            var salt = hasher.GenerateSalt();

            var adminGuid = Guid.NewGuid();

            adminUser = new User
            {
                Id = adminGuid,
                UserName = "admin",
                Salt = salt,
                Password = hasher.CreateSaltedHash("1234", salt),
                Emails = new List<Email>
                {
                    new Email
                    {
                        Address = "admin@etdb.com",
                        Id = Guid.NewGuid(),
                        IsPrimary = true,
                        UserId = adminGuid
                    }
                },
                SecurityRoleReferences = new List<MongoDBRef>
                {
                    new MongoDBRef($"{nameof(SecurityRole).ToLower()}s", memberRole.Id),
                    new MongoDBRef($"{nameof(SecurityRole).ToLower()}s", adminRole.Id)
                }
            };
            
            usersCollection.InsertOne(adminUser);
        }
    }
}