using System;
using System.Collections.Generic;
using System.Linq;
using Etdb.ServiceBase.Cryptography.Hashing;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Misc.Constants;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Etdb.UserService.Repositories
{
    public class ContextScaffold
    {
        private static readonly string[] Collections =
        {
            $"{nameof(User).ToLower()}s",
            $"{nameof(SecurityRole).ToLower()}s",
        };

        public static void Scaffold(UserServiceDbContext context)
        {
            foreach (var collectionName in ContextScaffold.Collections)
            {
                if (CollectionExists(collectionName, context.Database)) continue;

                CreateCollection(collectionName, context.Database);
            }

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
                DateTime.UtcNow,
                new[] {memberRole.Id, adminRole.Id},
                new List<Email> {new Email(Guid.NewGuid(), adminGuid, "admin@etdb.com", true, false)},
                password: hasher.CreateSaltedHash("1234", salt), salt: salt);

            usersCollection.InsertOne(adminUser);
        }

        private static bool CollectionExists(string collectionName, IMongoDatabase database)
        {
            var filter = new BsonDocument("name", collectionName);

            var collections = database.ListCollections(new ListCollectionsOptions
            {
                Filter = filter
            });

            return collections.Any();
        }

        private static void CreateCollection(string collectionName, IMongoDatabase database,
            CreateCollectionOptions options = null)
            => database.CreateCollection(collectionName, options);
    }
}