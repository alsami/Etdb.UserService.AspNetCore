using System;
using Etdb.UserService.Repositories.Conventions;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Etdb.UserService.Repositories
{
    public class UserServiceDbContext
    {
        public UserServiceDbContext(Func<IMongoDatabase> databaseComposer)
        {
            this.Database = databaseComposer();
            Configure();
        }

        public IMongoDatabase Database { get; }

        private static void Configure()
        {
            MongoDbConventions.UseGuidIdConvetion();
            MongoDbConventions.UseImmutableConvention();
            MongoDbConventions.UseCamelCaseConvention();
            MongoDbConventions.UseIgnoreNullValuesConvention();
            MongoDbConventions.UseEnumStringRepresentation();
        }
    }
}