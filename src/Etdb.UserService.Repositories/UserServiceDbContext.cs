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
            this.Configure();
        }

        public IMongoDatabase Database { get; }

        private void Configure()
        {
            MongoDbConventions.UseImmutableConvention();
            MongoDbConventions.UseCamelCaseConvention();
            MongoDbConventions.UseIgnoreNullValuesConvention();
            MongoDbConventions.UseEnumStringRepresentation();
            ConventionRegistry.Register(nameof(GuidIdConvention), new ConventionPack
            {
                new GuidIdConvention()
            }, _ => true);
        }
    }
}