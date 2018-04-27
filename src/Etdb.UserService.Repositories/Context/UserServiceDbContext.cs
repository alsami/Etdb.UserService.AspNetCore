using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.UserService.Domain;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Repositories.Context
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
            UseCamelCaseConvention();

            foreach (var collectionName in this.collections)
            {
                if (!this.CollectionExists(collectionName))
                {
                    this.CreateCollection(collectionName, AutoIndexIdCollectionOptions());
                }
            }
            
            ContextScaffold.Scaffold(this);
        }
    }
}