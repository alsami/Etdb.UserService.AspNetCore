using System.Collections.Generic;
using System.Threading.Tasks;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories;
using MongoDB.Driver;

namespace Etdb.UserService.Scaffolder.Migrations
{
    public class IndicesFactory
    {
        private readonly UserServiceDbContext context;

        public IndicesFactory(UserServiceDbContext context)
            => this.context = context;

        public async Task CreateIndicesAsync()
        {
            var tasks = new[]
            {
                this.CreateSecurityRolesIndices(),
                this.CreateUserIndices(),
            };

            await Task.WhenAll(tasks);
        }

        private async Task CreateUserIndices()
        {
            var usersCollection = this.context.Database.GetCollection<User>("users");

            var userNameIndex = new IndexKeysDefinitionBuilder<User>()
                .Ascending(user => user.UserName);

            var emailIndex = new IndexKeysDefinitionBuilder<User>()
                .Ascending(user => user.Emails);
            
            var authenticationLog = new IndexKeysDefinitionBuilder<User>()
                .Ascending(user => user.AuthenticationLogs);
            
            var profileImageIndex = new IndexKeysDefinitionBuilder<User>()
                .Ascending(user => user.ProfileImages);

            await usersCollection
                .Indexes
                .CreateManyAsync(new List<CreateIndexModel<User>>
                    {
                        new CreateIndexModel<User>(userNameIndex),
                        new CreateIndexModel<User>(emailIndex),
                        new CreateIndexModel<User>(authenticationLog),
                        new CreateIndexModel<User>(profileImageIndex),
                    }
                );
        }

        private async Task CreateSecurityRolesIndices()
        {
            var securityRolesCollection = this.context.Database.GetCollection<SecurityRole>("securityroles");

            var indexKeyDefinitions = new IndexKeysDefinitionBuilder<SecurityRole>()
                .Ascending(role => role.Name);

            await securityRolesCollection
                .Indexes
                .CreateOneAsync(new CreateIndexModel<SecurityRole>(indexKeyDefinitions));
        }
    }
}