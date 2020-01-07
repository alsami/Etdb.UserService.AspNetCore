using System;
using Etdb.ServiceBase.DocumentRepository;
using MongoDB.Driver;

namespace Etdb.UserService.Repositories
{
    public class UserServiceDbContext : DocumentDbContext
    {
        public UserServiceDbContext(Func<IMongoDatabase> databaseComposer) : base(databaseComposer)
        {
            this.Configure();
        }

        public sealed override void Configure()
        {
            MongoDbConventions.UseImmutableConvention();
            MongoDbConventions.UseCamelCaseConvention();
            MongoDbConventions.UseIgnoreNullValuesConvention();
            MongoDbConventions.UseEnumStringRepresentation();
        }
    }
}