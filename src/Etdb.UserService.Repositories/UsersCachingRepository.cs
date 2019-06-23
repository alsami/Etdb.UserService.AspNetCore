using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Extensions;
using Etdb.UserService.Repositories.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Etdb.UserService.Repositories
{
    public class UsersCachingRepository : GenericDocumentRepository<User, Guid>, IUsersRepository
    {
        private readonly IDistributedCache cache;

        public UsersCachingRepository(DocumentDbContext context, IDistributedCache cache) : base(context)
        {
            this.cache = cache;
        }

        public override async Task AddAsync(User user, string collectionName = null, string partitionKey = null)
        {
            await base.AddAsync(user, collectionName, partitionKey);

            await this.cache.AddAsync(user.Id, user);
        }

        public override async Task<bool> EditAsync(User user, string collectionName = null, string partitionKey = null)
        {
            if (!await base.EditAsync(user, collectionName, partitionKey)) return false;

            await this.cache.AddAsync(user.Id, user);

            return true;
        }

        public override async Task<User> FindAsync(Guid id, string collectionName = null, string partitionKey = null)
        {
            var cachedUser = await this.cache.FindAsync<User, Guid>(id);

            if (cachedUser != null)
            {
                return cachedUser;
            }

            var user = await base.FindAsync(id, collectionName, partitionKey);

            if (user == null) return null;

            await this.cache.AddAsync(user.Id, user);

            return user;
        }

        public override async Task<User> FindAsync(Expression<Func<User, bool>> predicate, string collectionName = null,
            string partitionKey = null)
        {
            var user = await base.FindAsync(predicate, collectionName, partitionKey);

            if (user == null) return null;

            await this.cache.AddAsync(user.Id, user);

            return user;
        }
    }
}