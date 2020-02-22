using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;
using MongoDB.Driver;

namespace Etdb.UserService.Repositories
{
    public class SecurityRolesRepository : ISecurityRolesRepository
    {
        private const string CollectionName = "securityroles";
        private readonly DocumentDbContext context;

        public SecurityRolesRepository(DocumentDbContext context)
        {
            this.context = context;
        }

        public Task<SecurityRole> FindAsync(Guid id)
        {
            return this.context.Database.GetCollection<SecurityRole>(CollectionName)
                .Find(role => role.Id == id)
                .SingleOrDefaultAsync();
        }

        public Task<SecurityRole> FindAsync(Expression<Func<SecurityRole, bool>> predicate)
        {
            return this.context.Database.GetCollection<SecurityRole>(CollectionName)
                .Find(predicate)
                .SingleOrDefaultAsync();
        }
    }
}