using System;
using System.Collections.Generic;
using System.Linq;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.UserService.Constants;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Repositories;
using MongoDB.Driver;

namespace Etdb.UserService.Scaffold
{
    internal class ContextScaffold
    {
        public static void Scaffold(UserServiceDbContext context)
        {
            var securityRoleCollection =
                context.Database.GetCollection<SecurityRole>($"{nameof(SecurityRole).ToLower()}s");

            var rolesToAdd = new List<SecurityRole>();

            var memberRole = securityRoleCollection.Find(role => role.Name == RoleNames.Member).FirstOrDefault();

            if (memberRole == null)
            {
                memberRole = new SecurityRole(Guid.NewGuid(), RoleNames.Member);

                rolesToAdd.Add(memberRole);
            }

            var adminRole = securityRoleCollection.Find(role => role.Name == RoleNames.Admin).FirstOrDefault();

            if (adminRole == null)
            {
                adminRole = new SecurityRole(Guid.NewGuid(), RoleNames.Admin);

                rolesToAdd.Add(adminRole);
            }

            if (rolesToAdd.Any())
            {
                securityRoleCollection.InsertMany(rolesToAdd);
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

            adminUser = new User(adminGuid, "admin", null, null, null,
                hasher.CreateSaltedHash("1234", salt), salt,
                DateTime.UtcNow,
                null,
                new[] {memberRole.Id, adminRole.Id},
                new List<Email> {new Email(Guid.NewGuid(), "admin@etdb.com", true)});

            usersCollection.InsertOne(adminUser);
        }
    }
}