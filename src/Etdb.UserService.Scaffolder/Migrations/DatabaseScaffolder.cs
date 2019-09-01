using System.Threading.Tasks;

namespace Etdb.UserService.Scaffolder.Migrations
{
    public class DatabaseScaffolder
    {
        private readonly CollectionsFactory collectionsFactory;
        private readonly IndicesFactory indicesFactory;
        private readonly DefaultDataFactory defaultDataFactory;

        public DatabaseScaffolder(CollectionsFactory collectionsFactory, IndicesFactory indicesFactory, DefaultDataFactory defaultDataFactory)
        {
            this.collectionsFactory = collectionsFactory;
            this.indicesFactory = indicesFactory;
            this.defaultDataFactory = defaultDataFactory;
        }

        public async Task ScaffoldAsync()
        {
            await this.collectionsFactory.CreateCollectionsAsync();
            await this.indicesFactory.CreateIndicesAsync();
            await this.defaultDataFactory.CreateDefaultDataAsync();
        }
    }
}