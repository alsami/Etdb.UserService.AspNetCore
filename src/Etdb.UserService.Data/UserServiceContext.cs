using Etdb.ServiceBase.Repositories.Abstractions.Base;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Etdb.UserService.Data
{
    public class UserServiceContext : AppContextBase
    {
        public override IMongoDatabase Database { get; }

        public UserServiceContext(IConfiguration configurationRoot)
        {
            var client = new MongoClient(configurationRoot.GetSection("MongoConnection:ConnectionString").Value);
            this.Database = client.GetDatabase(configurationRoot.GetSection("MongoConnection:Database").Value);
        }

        public override IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            return this.Database.GetCollection<TEntity>($"{typeof(TEntity).Name}s");
        }

        public override IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
        {
            return this.Database.GetCollection<TEntity>(collectionName);
        }

    }
}
