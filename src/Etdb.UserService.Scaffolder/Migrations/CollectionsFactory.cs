using System.Linq;
using System.Threading.Tasks;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Etdb.UserService.Scaffolder.Migrations
{
    public class CollectionsFactory
    {
        private readonly DocumentDbContext context;

        private static readonly string[] Collections =
        {
            $"{nameof(User).ToLower()}s",
            $"{nameof(SecurityRole).ToLower()}s",
        };

        public CollectionsFactory(DocumentDbContext context)
            => this.context = context;

        public async Task CreateCollectionsAsync()
        {
            var creationTasks = CollectionsFactory.Collections.Select(async collection =>
            {
                if (await CollectionExistsAsync(collection, this.context.Database)) return;

                await CreateCollection(collection, this.context.Database);
            });

            await Task.WhenAll(creationTasks);
        }

        private static async Task<bool> CollectionExistsAsync(string collectionName, IMongoDatabase database)
        {
            var filter = new BsonDocument("name", collectionName);

            var collections = await database.ListCollectionsAsync(new ListCollectionsOptions
            {
                Filter = filter
            });

            return collections.Any();
        }

        private static Task CreateCollection(string collectionName, IMongoDatabase database,
            CreateCollectionOptions? options = null)
            => database.CreateCollectionAsync(collectionName, options);
    }
}