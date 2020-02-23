using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.ValueObjects;
using Etdb.UserService.Extensions;
using Etdb.UserService.Repositories.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace Etdb.UserService.Repositories
{
    public class UsersCachingRepository : IUsersRepository
    {
        private const string CollectionName = "users";
        private readonly IDistributedCache cache;
        private readonly DocumentDbContext context;

        public UsersCachingRepository(DocumentDbContext context, IDistributedCache cache)
        {
            this.context = context;
            this.cache = cache;
        }

        public async Task AddAsync(User user)
        {
            await this.context.Database.GetCollection<User>(CollectionName)
                .InsertOneAsync(user)
                .ConfigureAwait(false);

            await this.cache.AddAsync(user.Id, user);
        }

        public async Task<bool> EditAsync(User user)
        {
            var editResult = await this.context.Database.GetCollection<User>(CollectionName)
                .ReplaceOneAsync(existingDocument => existingDocument.Id == user.Id, user)
                .ConfigureAwait(false);

            if (editResult.ModifiedCount == 0) return false;

            await this.cache.AddAsync(user.Id, user);

            return true;
        }

        public async Task<User?> FindAsync(Guid id)
        {
            var cachedUser = await this.cache.FindAsync<User, Guid>(id);

            if (cachedUser != null) return cachedUser;

            var user = await this.context.Database.GetCollection<User>(CollectionName)
                .Find(document => document.Id == id)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            // var user = await base.FindAsync(id, collectionName, partitionKey);

            if (user == null) return null;

            await this.cache.AddAsync(user.Id, user);

            return user;
        }

        public async Task<User?> FindAsync(Expression<Func<User, bool>> predicate)
        {
            var user = await this.context.Database.GetCollection<User>(CollectionName)
                .Find(predicate)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            if (user == null) return null;

            await this.cache.AddAsync(user.Id, user);

            return user;
        }

        public async Task<IEnumerable<User>> FindAllAsync(Expression<Func<User, bool>> predicate)
        {
            var users = await this.context.Database.GetCollection<User>(CollectionName)
                .Find(predicate)
                .ToListAsync()
                .ConfigureAwait(false);

            return users;
        }
        
        public Email? FindEmailAddress(string emailAddress)
        {
            var email = this.context.Database.GetCollection<User>(CollectionName)
                .AsQueryable()
                .SelectMany(user => user.Emails)
                .FirstOrDefault(EmailEqualsExpression(emailAddress));

            return email;
        }
        
        private static Expression<Func<Email, bool>> EmailEqualsExpression(string emailAddress)
        {
            return email => email.Address.ToLower() == emailAddress.ToLower();
        }
    }
}