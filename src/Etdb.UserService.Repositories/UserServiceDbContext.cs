using Etdb.ServiceBase.DocumentRepository;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Etdb.UserService.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Etdb.UserService.Repositories
{
    public class UserServiceDbContext : DocumentDbContext
    {
        private readonly string[] collections =
        {
            $"{nameof(User).ToLower()}s",
            $"{nameof(SecurityRole).ToLower()}s",
            $"{nameof(AuthenticationLog).ToLower()}s"
        };

        public UserServiceDbContext(IOptions<DocumentDbContextOptions> options) : base(options)
        {
            this.Configure();
        }

        public sealed override void Configure()
        {
            UseImmutableConvention();
            UseCamelCaseConvention();
            UseIgnoreNullValuesConvention();
            UseEnumStringRepresentation();

            //foreach (var collectionName in this.collections)
            //{
            //    if (!this.CollectionExists(collectionName))
            //    {
            //        this.CreateCollection(collectionName);
            //    }
            //}
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