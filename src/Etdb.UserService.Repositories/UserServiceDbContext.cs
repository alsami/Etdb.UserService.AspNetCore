using Etdb.ServiceBase.DocumentRepository;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Etdb.UserService.Repositories
{
    public class UserServiceDbContext : DocumentDbContext
    {
        public UserServiceDbContext(IOptions<DocumentDbContextOptions> options) : base(options)
            => this.Configure();

        public UserServiceDbContext(MongoClientSettings settings, string databaseName) : base(settings, databaseName)
            => this.Configure();

        public sealed override void Configure()
        {
            MongoDbConventions.UseImmutableConvention();
            MongoDbConventions.UseCamelCaseConvention();
            MongoDbConventions.UseIgnoreNullValuesConvention();
            MongoDbConventions.UseEnumStringRepresentation();
        }
    }
}