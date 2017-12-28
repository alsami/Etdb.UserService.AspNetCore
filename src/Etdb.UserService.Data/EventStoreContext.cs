using Etdb.ServiceBase.EventSourcing.Abstractions.Base;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Etdb.UserService.Data
{
    public class EventStoreContext : EventStoreContextBase
    {
        public EventStoreContext(IConfiguration configurationRoot)
        {
            var client = new MongoClient(configurationRoot.GetSection("MongoConnection:ConnectionString").Value);
            this.Database = client.GetDatabase(configurationRoot.GetSection("MongoConnection:Database").Value);
        }

        public override IMongoDatabase Database { get; }
    }
}
