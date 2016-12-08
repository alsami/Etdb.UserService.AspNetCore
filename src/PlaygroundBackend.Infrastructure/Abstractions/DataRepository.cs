using PlaygroundBackend.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using PlaygroundBackend.Persistency;

namespace PlaygroundBackend.Infrastructure.Abstractions
{
    public class DataRepository<T> : IDataRepository<T> where T: class, IPersistedData, new()
    {
        private readonly PlaygroundContext context;

        public DataRepository(PlaygroundContext context)
        {
            this.context = context;
        }

        public virtual void Add(T data)
        {
            EntityEntry entry = this.context.Entry(data);
            this.context.Set<T>().Add(data);
        }

        public virtual void Delete(T data)
        {
            EntityEntry entry = this.context.Entry(data);
            entry.State = EntityState.Deleted;
        }

        public virtual void Edit(T data)
        {
            EntityEntry entry = this.context.Entry(data);
            entry.State = EntityState.Modified;
        }

        public virtual void EnsureChanges()
        {
            try
            {
                this.context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public virtual T Get(int id)
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

        public virtual T GetIncluding(int id, params Expression<Func<T, object>>[] includes)
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

        public virtual IQueryable<T> GetQueryable()
        {
            return this.context
                .Set<T>()
                .AsQueryable();
        }
    }
}
