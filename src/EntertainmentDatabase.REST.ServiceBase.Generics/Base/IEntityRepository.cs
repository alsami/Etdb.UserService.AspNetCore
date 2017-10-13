using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Base
{
    public interface IEntityRepository<TEntity> where TEntity: class, IEntity, new()
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        TEntity Get(Guid id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        TEntity GetIncluding(Guid id, params Expression<Func<TEntity, object>>[] includes);
        TEntity GetIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        IQueryable<TEntity> GetQueryable();
        void Add(TEntity entity);
        void Edit(TEntity entity);
        void Delete(TEntity entity);
        int EnsureChanges();
        Task<int> EnsureChangesAsync();
    }
}
