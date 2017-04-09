using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstraction;

namespace EntertainmentDatabase.REST.ServiceBase.DataAccess.Abstraction
{
    public interface IEntityRepository<T> where T: class, IEntity, new()
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetAllIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        T Get(Guid id);
        T Get(Expression<Func<T, bool>> predicate);
        T GetIncluding(Guid id, params Expression<Func<T, object>>[] includes);
        T GetIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetQueryable();
        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);
        void EnsureChanges();
    }
}
