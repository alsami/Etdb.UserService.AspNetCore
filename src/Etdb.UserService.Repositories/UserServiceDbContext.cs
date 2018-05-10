using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.UserService.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace Etdb.UserService.Repositories
{
    public class UserServiceDbContext : DocumentDbContext
    {
        private readonly string[] collections = new[]
        {
            $"{nameof(User).ToLower()}s",
            $"{nameof(SecurityRole).ToLower()}s"
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

            foreach (var collectionName in this.collections)
            {
                if (!this.CollectionExists(collectionName))
                {
                    this.CreateCollection(collectionName, AutoIndexIdCollectionOptions());
                }
            }
        }

        private static void UseIgnoreNullValuesConvention()
        {
            ConventionRegistry.Register(nameof(IgnoreIfDefaultConvention), 
                new ConventionPack { new IgnoreIfDefaultConvention(true) }, type => true);
        }
    }
}