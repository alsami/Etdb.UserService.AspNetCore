using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UltimateCoreWebAPI.Model.Abstractions;
using UltimateCoreWebAPI.Persistency;

namespace UltimateCoreWebAPI.Infrastructure.Abstractions
{
    public class EntityRepository<T> : IEntityRepository<T> where T: class, IEntity, new()
    {
        private readonly CoreWebAPIContext context;

        public EntityRepository(CoreWebAPIContext context)
        {
            this.context = context;
        }

        public virtual void Add(T entity)
        {
            EntityEntry entry = this.context.Entry(entity);
            this.context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            EntityEntry entry = this.context.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        public virtual void Edit(T entity)
        {
            EntityEntry entry = this.context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public virtual void EnsureChanges()
        {
            this.context.SaveChanges();
        }


        public virtual T Get(Guid id)
        {
            return this.context
                .Set<T>()
                .FirstOrDefault(data => data.Id == id);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return this.context
                .Set<T>()
                .Where(predicate)
                .FirstOrDefault();
        }

        public virtual T GetIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = this.context
                .Set<T>()
                .AsQueryable();

            query = includes
                .Aggregate(query, (current, include) => current.Include(include));

            return query
                .Where(predicate)
                .FirstOrDefault();
        }

        public virtual T GetIncluding(Guid id, params Expression<Func<T, object>>[] includes)
        {
            var query = this.context
                .Set<T>()
                .AsQueryable();

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query
                .FirstOrDefault(data => data.Id == id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.context
                .Set<T>()
                .AsEnumerable();
        }

        public virtual IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includes)
        {
            var query = this.context
                .Set<T>()
                .AsQueryable();

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query.AsEnumerable();
        }

        public IEnumerable<T> GetAllIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = this.context
                .Set<T>()
                .AsQueryable();

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query
                .Where(predicate)
                .AsEnumerable();
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return this.context
                .Set<T>()
                .AsQueryable();
        }

        private IQueryable<T> BuildQueryWithIncludes(params Expression<Func<T, object>>[] includes)
        {
            var query = this.context
                .Set<T>()
                .AsQueryable();

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return includes.Aggregate(query, (current, include) => current.Include(include));
        }
    }
}
