using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EntertainmentDatabase.Rest.DataAccess.Abstraction;
using EntertainmentDatabase.REST.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace EntertainmentDatabase.Rest.DataAccess.Facade
{
    public class EntityRepository<T> : IEntityRepository<T> where T: class, IEntity, new()
    {
        private readonly DbContext context;

        public EntityRepository(DbContext context)
        {
            this.context = context;
        }

        public virtual void Add(T entity)
        {
            var entry = this.context.Entry(entity);
            this.context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            var entry = this.context.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        public virtual void Edit(T entity)
        {
            var entry = this.context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public virtual void EnsureChanges()
        {
            this.context.SaveChanges();
        }

        public virtual T Get(Guid id)
        {
            return this.GetQuery()
                .FirstOrDefault(entity => entity.Id == id);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return this.GetQuery()
                .FirstOrDefault(predicate);
        }

        public virtual T GetIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return this.BuildQuery(includes)
                .Where(predicate)
                .FirstOrDefault();
        }

        public virtual T GetIncluding(Guid id, params Expression<Func<T, object>>[] includes)
        {
            return this.BuildQuery(includes)
                .FirstOrDefault(data => data.Id == id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.GetQuery();
        }

        public virtual IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includes)
        {
            return this.BuildQuery(includes)
                .AsEnumerable();
        }

        public IEnumerable<T> GetAllIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return this.BuildQuery(includes)
                .Where(predicate)
                .AsEnumerable();
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return this.context
                .Set<T>()
                .AsQueryable();
        }

        private IQueryable<T> BuildQuery(params Expression<Func<T, object>>[] includes)
        {
            var query = this.GetQuery();

            return includes.Aggregate(query, (current, include) => current.Include(include));
        }

        private IQueryable<T> GetQuery()
        {
            return this.context
                .Set<T>()
                .AsQueryable();
        }
    }
}
