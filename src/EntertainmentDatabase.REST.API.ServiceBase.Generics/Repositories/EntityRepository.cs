using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using Microsoft.EntityFrameworkCore;

namespace EntertainmentDatabase.REST.API.ServiceBase.Generics.Repositories
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity: class, IEntity, new()
    {
        private readonly DbContext context;

        public EntityRepository(DbContext context)
        {
            this.context = context;
        }

        public virtual void Add(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            var entry = this.context.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        public virtual void Edit(TEntity entity)
        {
            var entry = this.context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public virtual int EnsureChanges()
        {
            return this.context.SaveChanges();
        }

        public virtual async Task<int> EnsureChangesAsync()
        {
            return await this.context.SaveChangesAsync();
        }

        public virtual TEntity Get(Guid id)
        {
            return this.GetQuery()
                .FirstOrDefault(entity => entity.Id == id);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this.GetQuery()
                .FirstOrDefault(predicate);
        }

        public virtual TEntity GetIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return this.BuildQuery(includes)
                .FirstOrDefault(predicate);
        }

        public virtual TEntity GetIncluding(Guid id, params Expression<Func<TEntity, object>>[] includes)
        {
            return this.BuildQuery(includes)
                .FirstOrDefault(data => data.Id == id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return this.GetQuery();
        }

        public virtual IEnumerable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includes)
        {
            return this.BuildQuery(includes)
                .AsEnumerable();
        }

        public IEnumerable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return this.BuildQuery(includes)
                .Where(predicate)
                .AsEnumerable();
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return this.context
                .Set<TEntity>()
                .AsQueryable();
        }

        private IQueryable<TEntity> BuildQuery(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = this.GetQuery();

            return includes.Aggregate(query, (current, include) => current.Include(include));
        }

        private IQueryable<TEntity> GetQuery()
        {
            return this.context
                .Set<TEntity>()
                .AsQueryable();
        }
    }
}
