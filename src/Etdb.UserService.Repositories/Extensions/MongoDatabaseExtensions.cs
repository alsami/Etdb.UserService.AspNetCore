using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Etdb.UserService.Repositories.Extensions
{
    public static class MongoDatabaseExtensions
    {
        public static bool CollectionExists(this IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);

            var collections = database.ListCollections(new ListCollectionsOptions
            {
                Filter = filter
            });

            return collections.Any();
        }

        public static async Task<bool> CollectionExistsAsync(this IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);

            var collections = await database.ListCollectionsAsync(new ListCollectionsOptions
            {
                Filter = filter
            });

            return collections.Any();
        }

        public static void CreateCollection(IMongoDatabase database, string collectionName,
            CreateCollectionOptions? options = null)
            => database.CreateCollection(collectionName, options);

        public static Task CreateCollectionAsync(IMongoDatabase database, string collectionName,
            CreateCollectionOptions? options = null)
            => database.CreateCollectionAsync(collectionName, options);
    }
}