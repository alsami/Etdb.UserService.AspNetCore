using Etdb.ServiceBase.DocumentRepository;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Etdb.UserService.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
            UseImmutableConvention();
            UseCamelCaseConvention();
            UseIgnoreNullValuesConvention();
            UseEnumStringRepresentation();
        }

        private static void UseIgnoreNullValuesConvention()
        {
            ConventionRegistry.Register(nameof(IgnoreIfDefaultConvention),
                new ConventionPack {new IgnoreIfDefaultConvention(true)}, _ => true);
        }

        private static void UseEnumStringRepresentation()
        {
            ConventionRegistry.Register(nameof(EnumRepresentationConvention), new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            }, _ => true);
        }
    }
}