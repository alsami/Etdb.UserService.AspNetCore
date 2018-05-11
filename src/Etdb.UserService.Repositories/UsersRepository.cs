using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.DocumentRepository.Generics;
using Etdb.UserService.Domain;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories
{
    public class UsersRepository : GenericDocumentRepository<User, Guid>, IUsersRepository
    {
        private readonly DocumentDbContext context;

        public UsersRepository(DocumentDbContext context) : base(context)
        {
            this.context = context;
        }
    }
//
//    public class UsersRepositoryWithCache : UsersRepository
//    {
//        private readonly IDistributedCache cache;
//
//        public UsersRepositoryWithCache(DocumentDbContext context, IDistributedCache cache) : base(context)
//        {
//            this.cache = cache;
//        }
//
//        public override Task AddAsync(User document, string collectionName = null, string partitionKey = null)
//        {
//            
//            var ret = base.AddAsync(document, collectionName, partitionKey);
////this.cache.AddOrUpdateAsync()
//            return ret;
//            
//            
//        }
//
//        public override User Find(Guid id, string collectionName = null, string partitionKey = null)
//        {
//            return base.Find(id, collectionName, partitionKey);
//        }
//    }
}