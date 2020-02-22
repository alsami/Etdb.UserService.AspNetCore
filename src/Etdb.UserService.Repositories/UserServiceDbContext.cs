using System;
using Etdb.ServiceBase.DocumentRepository;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
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
            ConventionRegistry.Register(nameof(GuidIdConvention), new ConventionPack
            {
                new GuidIdConvention()
            }, _ => true);
        }
    }
    
    public class GuidIdConvention : ConventionBase, IPostProcessingConvention {
        public void PostProcess(BsonClassMap classMap) {
            var idMap = classMap.IdMemberMap;
            if (idMap == null || idMap.MemberName != "Id" || idMap.MemberType != typeof(Guid)) return;
            
            idMap.SetIdGenerator(new GuidGenerator());
        }
    }
}